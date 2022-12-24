namespace EasyCards.Bootstrap;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Logging;
using CardTypes;
using Effects;
using Extensions;
using Helpers;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Models.Templates;
using ModGenesia;
using RogueGenesia.Data;
using Services;
using UnityEngine;
using Enum = System.Enum;
using Exception = System.Exception;
using ModGenesia = ModGenesia.ModGenesia;

public static class CardLoader
{
    static CardLoader()
    {
        Logger = EasyCards.Instance.Log;
        _placeholderSprite = SpriteLoader.LoadSprite(Path.Combine(Paths.EasyCards, "placeholder.png"));
    }

    private static ManualLogSource Logger { get; }
    private static readonly Sprite _placeholderSprite;

    private static readonly Dictionary<string, CardTemplate> _successFullyLoadedCards = new();

    public static void Initialize()
    {
        RegisterCustomCardTypes();
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

        var allCards = CardRepository.GetAllCards().ToDictionary(card => card.name);
        PostProcessRemovals(allCards, _successFullyLoadedCards);
        PostProcessRequirements(allCards, _successFullyLoadedCards);
    }

    private static void RegisterCustomCardTypes()
    {
        if (!ClassInjector.IsTypeRegisteredInIl2Cpp<ConfigurableEffectCard>())
        {
            ClassInjector.RegisterTypeInIl2Cpp<ConfigurableEffectCard>();
        }
    }

    public static Dictionary<string, CardTemplate> GetLoadedCards() => _successFullyLoadedCards;

    private static void AddCardsFromFile(string fileName, string assetBasePath)
    {
        if (!File.Exists(fileName))
        {
            Logger.LogError($"File does not exist: {fileName}");
        }

        Logger.LogInfo($"Loading cards from file {fileName}");

        var json = File.ReadAllText(fileName);
        var templateFile = JsonDeserializer.Deserialize<TemplateFile>(json);

        Logger.LogInfo($"Loaded {templateFile.Stats.Count} cards");

        var modSource = templateFile.ModSource ?? MyPluginInfo.PLUGIN_NAME;

        var effectCardType = Il2CppType.Of<ConfigurableEffectCard>();
        var effectCardConstructor = effectCardType.GetConstructors().First();

        var allCards = templateFile.Stats
            .Concat(templateFile.StatCards)
            .ToList();

        foreach (var cardTemplate in allCards)
        {
            try
            {
                var soulCardData = ConvertCardTemplate(cardTemplate, modSource, assetBasePath);

                if (cardTemplate.Effects.Count == 0)
                {
                    Logger.LogInfo($"Adding stat card {cardTemplate.Name}");
                    ModGenesia.AddCustomStatCard(cardTemplate.Name, soulCardData);
                }
                else
                {
                    Logger.LogInfo($"Adding effect card {cardTemplate.Name}");
                    ModGenesia.AddCustomCard(cardTemplate.Name, effectCardConstructor, soulCardData);

                    foreach (var effect in cardTemplate.Effects)
                    {
                        effect.AssetBasePath = assetBasePath;
                        EffectHolder.AddEffect(cardTemplate.Name, effect);
                    }
                }

                _successFullyLoadedCards.Add(cardTemplate.Name, cardTemplate);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error adding {cardTemplate.Name}: {ex}");
            }
        }
    }

    private static SoulCardCreationData ConvertCardTemplate(CardTemplate cardTemplate, string modSource,
        string assetBasePath)
    {
        var soulCardData = new SoulCardCreationData();

        soulCardData.ModSource = modSource;

        Sprite? sprite = null;

        if (cardTemplate.TexturePath != null)
        {
            var texturePath = Path.Combine(assetBasePath, cardTemplate.TexturePath);
            sprite = SpriteLoader.LoadSprite(texturePath);
            if (sprite)
            {
                soulCardData.Texture = sprite;
            }
            else
            {
                soulCardData.Texture = _placeholderSprite;
                Logger.LogError($"Unable to load sprite from {texturePath}. Assigning placeholder sprite.");
            }
        }
        else
        {
            Logger.LogError($"No sprite set for {cardTemplate.Name}. Assigning placeholder sprite.");
            soulCardData.Texture = _placeholderSprite;
        }


        soulCardData.Rarity = (CardRarity)(int)cardTemplate.Rarity;

        var tags = (CardTag)cardTemplate.CardTags.Aggregate(0, (current, tag) => current | (int)tag);

        soulCardData.Tags = tags;
        soulCardData.DropWeight = cardTemplate.DropWeight;
        soulCardData.LevelUpWeight = cardTemplate.LevelUpWeight;
        soulCardData.MaxLevel = cardTemplate.MaxLevel;

        soulCardData.StatsModifier = cardTemplate.CreateStatsModifier();

        // ModifyPlayerStat controls if stats are shown on a card.
        // Make sure it's set to true if we have modifiers.
        // Additionally, allow card creators to disable it.
        soulCardData.ModifyPlayerStat = cardTemplate.Modifiers.Count > 0;

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
            soulCardData.CardWithStatsToBan = ConvertStringsToStatsTypes(cardTemplate.BanishesCardsWithStatsOfType);
        }

        // soulCardData.CardRequirement = cardTemplate.RequiresAny?.ToRequirementList();
        // soulCardData.CardHardRequirement = cardTemplate.RequiresAll?.ToRequirementList();

        return soulCardData;
    }

    private static void PostProcessRemovals(Dictionary<string, SoulCardScriptableObject> allCards,
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

    private static void PostProcessRequirements(Dictionary<string, SoulCardScriptableObject> allCards,
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
            }

            if (cardTemplate.RequiresAll != null)
            {
                // Logger.LogDebug($"\t{cardName} - RequiresAll");
                cardScso.HardCardRequirement = cardTemplate.RequiresAll.ToRequirementList();
            }
        }
    }


    private static List<SoulCardScriptableObject> GetCardsForIdentifiers(
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

    private static StatsType[] ConvertStringsToStatsTypes(List<string> statNames)
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
