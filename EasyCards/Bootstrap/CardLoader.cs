using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EasyCards.Helpers;
using EasyCards.Models.Templates;
using EasyCards.Services;
using ModGenesia;
using RogueGenesia.Data;

namespace EasyCards.Bootstrap;

using BepInEx.Logging;
using Extensions;
using Logging;
using UnityEngine;

public sealed class CardLoader : ICardLoader
{
    public CardLoader(IJsonDeserializer jsonDeserializer, IDebugHelper debugHelper,
        ISpriteLoader spriteLoader, ICardRepository cardRepository, ILoggerConfiguration loggerConfiguration)
    {
        Logger = EasyCards.Instance.Log;
        _jsonDeserializer = jsonDeserializer;
        _debugHelper = debugHelper;
        _spriteLoader = spriteLoader;
        _cardRepository = cardRepository;
        this._placeholderSprite = spriteLoader.LoadSprite(Path.Combine(Paths.EasyCards, "placeholder.png"));
        this.loggerConfiguration = loggerConfiguration;
    }

    public int LoadOrder => 75;

    private ManualLogSource Logger { get; }
    private readonly IJsonDeserializer _jsonDeserializer;
    private readonly IDebugHelper _debugHelper;
    private readonly ISpriteLoader _spriteLoader;
    private readonly ICardRepository _cardRepository;
    private readonly ILoggerConfiguration loggerConfiguration;
    private readonly Sprite _placeholderSprite;

    private readonly Dictionary<string, CardTemplate> _successFullyLoadedCards = new();

    public void Initialize()
    {
        if (Directory.Exists(Paths.Data))
        {
            var jsonFiles = Directory.GetFiles(Paths.Data, "*.json");

            // Load files using the old logic
            foreach (var jsonFile in jsonFiles)
            {
                try
                {
                    AddCardsFromFile(jsonFile, Paths.Assets);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Unable to load cards from file {jsonFile}: {ex}");
                }
            }
        }

        // Scan for *.cards.json files in plugins subfolders
        var cardJsonFiles = Directory.GetFiles(Paths.Plugins, "*.cards.json", SearchOption.AllDirectories);
        foreach (var jsonFile in cardJsonFiles)
        {
            try
            {
                var assetPath = Path.GetDirectoryName(jsonFile);
                AddCardsFromFile(jsonFile, assetPath!);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Unable to load cards from file {jsonFile}: {ex}");
            }
        }

        var allCards = _cardRepository.GetAllCards().ToDictionary(card => card.name);
        PostProcessRemovals(allCards, _successFullyLoadedCards);
        PostProcessRequirements(allCards, _successFullyLoadedCards);
    }

    public Dictionary<string, CardTemplate> GetLoadedCards() => _successFullyLoadedCards;

    public void AddCardsFromFile(string fileName, string assetBasePath)
    {
        if (!File.Exists(fileName))
        {
            Logger.LogError($"File does not exist: {fileName}");
        }

        Logger.LogInfo($"Loading cards from file {fileName}");

        var json = File.ReadAllText(fileName);
        var templateFile = _jsonDeserializer.Deserialize<TemplateFile>(json);

        Logger.LogInfo($"Loaded {templateFile.Stats.Count} cards");

        var modSource = templateFile.ModSource ?? MyPluginInfo.PLUGIN_NAME;

        foreach (var cardTemplate in templateFile.Stats)
        {
            try
            {
                var soulCardData = this.ConvertCardTemplate(cardTemplate, modSource, assetBasePath);
                Logger.LogInfo($"Adding card {cardTemplate.Name}");
                ModGenesia.ModGenesia.AddCustomStatCard(cardTemplate.Name, soulCardData);
                _successFullyLoadedCards.Add(cardTemplate.Name, cardTemplate);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error adding {cardTemplate.Name}: {ex}");
            }
        }
    }

    private SoulCardCreationData ConvertCardTemplate(CardTemplate cardTemplate, string modSource, string assetBasePath)
    {
        if (loggerConfiguration.IsLoggerEnabled())
        {
            Logger.LogDebug($"Converting {cardTemplate.Name}");
        }

        var soulCardData = new SoulCardCreationData();

        soulCardData.ModSource = modSource;

        Sprite? sprite = null;

        if (cardTemplate.TexturePath != null)
        {
            var texturePath = Path.Combine(assetBasePath, cardTemplate.TexturePath);
            sprite = _spriteLoader.LoadSprite(texturePath);
            if (sprite)
            {
                soulCardData.Texture = sprite;
            }
            else
            {
                soulCardData.Texture = this._placeholderSprite;
                Logger.LogError($"Unable to load sprite from {texturePath}. Assigning placeholder sprite.");
            }
        }
        else
        {
            Logger.LogError($"No sprite set for {cardTemplate.Name}. Assigning placeholder sprite.");
            soulCardData.Texture = this._placeholderSprite;
        }


        soulCardData.Rarity = (CardRarity)(int)cardTemplate.Rarity;

        var tags = (CardTag)cardTemplate.Tags.Aggregate(0, (current, tag) => current | (int)tag);

        soulCardData.Tags = tags;
        soulCardData.DropWeight = cardTemplate.DropWeight;
        soulCardData.LevelUpWeight = cardTemplate.LevelUpWeight;
        soulCardData.MaxLevel = cardTemplate.MaxLevel;

        soulCardData.StatsModifier = cardTemplate.CreateStatsModifier();

        if (cardTemplate.NameLocalization.Count > 0)
        {
            var nameLocalizations = Localization.GetNameTranslations(cardTemplate);
            foreach (var localization in nameLocalizations)
            {
                soulCardData.NameOverride.Add(localization);
            }
        }
        else
        {
            Logger.LogWarning($"No Name localizations provided for {cardTemplate.Name}!");
        }

        if (cardTemplate.DescriptionLocalization.Count > 0)
        {
            var nameLocalizations = Localization.GetDescriptionTranslations(cardTemplate);
            foreach (var localization in nameLocalizations)
            {
                soulCardData.DescriptionOverride.Add(localization);
            }
        }

        soulCardData.DisableInRogMode = cardTemplate.DisabledInMode == DisabledInMode.Rogs;
        soulCardData.DisableInSurvivorsMode = cardTemplate.DisabledInMode == DisabledInMode.Survivors;

        soulCardData.CardExclusion = cardTemplate.BanishesCardsByName.ToArray();
        // soulCardData.CardToRemove = cardTemplate.RemovesCards.ToArray();
        if (cardTemplate.BanishesCardsWithStatsOfType.Count > 0)
        {
            soulCardData.CardWithStatsToBan = this.ConvertStringsToStatsTypes(cardTemplate.BanishesCardsWithStatsOfType);
        }

        // soulCardData.CardRequirement = cardTemplate.RequiresAny?.ToRequirementList();
        // soulCardData.CardHardRequirement = cardTemplate.RequiresAll?.ToRequirementList();

        return soulCardData;
    }

    private void PostProcessRemovals(Dictionary<string, SoulCardScriptableObject> allCards,
        Dictionary<string, CardTemplate> addedCards)
    {
        Logger.LogDebug($"=== Post processing removals for {addedCards.Count} cards ===");

        var addedCardNames = addedCards.Keys;
        foreach (var cardName in addedCardNames)
        {
            Logger.LogDebug($"Processing {cardName}");
            var cardTemplate = addedCards[cardName];
            var cardScso = allCards[cardName];

            var cardsToRemove = GetCardsForIdentifiers(allCards, cardTemplate.RemovesCards);

            if (cardsToRemove.Count > 0)
            {
                Logger.LogDebug($"\tRemoves {cardsToRemove.Count} cards:");
                foreach (var cardToRemove in cardsToRemove)
                {
                    Logger.LogDebug($"\t\t{cardToRemove.name}");
                }
            }

            cardScso.CardToRemove = cardsToRemove.ToIl2CppReferenceArray();
        }
    }

    private void PostProcessRequirements(Dictionary<string, SoulCardScriptableObject> allCards,
    Dictionary<string, CardTemplate> addedCards)
    {
        // Logger.LogDebug($"=== Post processing requirements for {addedCards.Count} cards ===");

        var addedCardNames = addedCards.Keys;
        foreach (var cardName in addedCardNames)
        {
            // Logger.LogDebug($"Processing {cardName}");
            var cardTemplate = addedCards[cardName];
            var cardScso = allCards[cardName];

            if (cardTemplate.RequiresAny != null)
            {
                // Logger.LogDebug($"\t{cardName} - RequiresAny");
                cardScso.CardRequirement = cardTemplate.RequiresAny.ToRequirementList();
                _debugHelper.LogRequirements(cardScso.CardRequirement, "\t\t");
            }

            if (cardTemplate.RequiresAll != null)
            {
                // Logger.LogDebug($"\t{cardName} - RequiresAll");
                cardScso.HardCardRequirement = cardTemplate.RequiresAll.ToRequirementList();
                _debugHelper.LogRequirements(cardScso.HardCardRequirement, "\t\t");
            }
        }
    }


    private List<SoulCardScriptableObject> GetCardsForIdentifiers(
        Dictionary<string, SoulCardScriptableObject> allCards, List<string> cardsToGet)
    {
        var result = new List<SoulCardScriptableObject>();
        foreach (var cardToGet in cardsToGet)
        {
            var cardScso = allCards.GetValueOrDefault(cardToGet);
            if (cardScso != null)
            {
                result.Add(cardScso);
            }
        }

        return result;
    }

    private StatsType[] ConvertStringsToStatsTypes(List<string> statNames)
    {
        var result = new HashSet<StatsType>();
        foreach (var statName in statNames)
        {
            if (Enum.TryParse<StatsType>(statName, true, out var stat))
            {
                result.Add(stat);
            }
            else
            {
                Logger.LogWarning($"Could not convert stat: {statName} ");
            }
        }

        return result.ToArray();
    }
}
