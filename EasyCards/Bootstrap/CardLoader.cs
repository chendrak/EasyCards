namespace EasyCards.Bootstrap;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardTypes;
using Common.Logging;
using Effects;
using Helpers;
using Models.Templates;
using ModGenesia;
using RogueGenesia.Data;
using Services;
using UnityEngine;
using Enum = System.Enum;
using Exception = System.Exception;

public static class CardLoader
{
    static CardLoader()
    {
        _placeholderSprite = ModGenesia.LoadSprite(Path.Combine(Paths.EasyCards, "placeholder.png"));
    }

    private static readonly Sprite _placeholderSprite;

    private static readonly Dictionary<string, CardTemplate> _successFullyLoadedCards = new();

    public static void Initialize(List<string> modPaths)
    {

        foreach (var modPath in modPaths)
        {

            Log.Info($"Searching for cards.json files in {modPath}");

            // Scan for *.cards.json files in plugins subfolders
            var cardJsonFiles = Directory.GetFiles(modPath, "*.cards.json", SearchOption.AllDirectories);
            foreach (var jsonFile in cardJsonFiles)
            {
                try
                {
                    var assetPath = Path.GetDirectoryName(jsonFile);
                    AddCardsFromFile(jsonFile, assetPath!);
                }
                catch (Exception ex)
                {
                    Log.Error($"Unable to load cards from file {jsonFile}: {ex}");
                }
            }
        }

        var allCards = CardRepository.GetAllCards().ToDictionary(card => card.name);
        PostProcessRemovals(allCards, _successFullyLoadedCards);
        PostProcessRequirements(allCards, _successFullyLoadedCards);
    }

    public static Dictionary<string, CardTemplate> GetLoadedCards() => _successFullyLoadedCards;

    private static void AddCardsFromFile(string fileName, string assetBasePath)
    {
        if (!File.Exists(fileName))
        {
            Log.Error($"File does not exist: {fileName}");
        }

        Log.Info($"Loading cards from file {fileName}");

        var json = File.ReadAllText(fileName);
        var templateFile = JsonDeserializer.Deserialize<TemplateFile>(json);

        Log.Info($"Loaded {templateFile.Stats.Count} cards");

        var modSource = templateFile.ModSource ?? EasyCards.MOD_NAME;

        var effectCardType = typeof(ConfigurableEffectCard);
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
                    Log.Debug($"Adding stat card {cardTemplate.Name}");
                    CardAPI.AddCustomStatCard(cardTemplate.Name, soulCardData);
                }
                else
                {
                    Log.Debug($"Adding effect card {cardTemplate.Name}");
                    CardAPI.AddCustomCard(cardTemplate.Name, effectCardConstructor, soulCardData);

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
                Log.Error($"Error adding {cardTemplate.Name}: {ex}");
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
            sprite = ModGenesia.LoadSprite(texturePath);
            if (sprite)
            {
                soulCardData.Texture = sprite;
            }
            else
            {
                soulCardData.Texture = _placeholderSprite;
                Log.Error($"Unable to load sprite from {texturePath}. Assigning placeholder sprite.");
            }
        }
        else
        {
            Log.Error($"No sprite set for {cardTemplate.Name}. Assigning placeholder sprite.");
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
            Log.Warn($"No Name localizations provided for {cardTemplate.Name}!");
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
        Log.Debug($"=== Post processing removals for {addedCards.Count} cards ===");

        var addedCardNames = addedCards.Keys;
        foreach (var cardName in addedCardNames)
        {
            Log.Debug($"Processing {cardName}");
            var cardTemplate = addedCards[cardName];
            var cardScso = allCards[cardName];

            var cardsToRemove = GetCardsForIdentifiers(allCards, cardTemplate.RemovesCards);

            if (cardsToRemove.Count > 0)
            {
                Log.Debug($"\tRemoves {cardsToRemove.Count} cards:");
                foreach (var cardToRemove in cardsToRemove)
                {
                    Log.Debug($"\t\t{cardToRemove.name}");
                }
            }

            cardScso.CardRemoved = cardsToRemove.ToArray();
        }
    }

    private static void PostProcessRequirements(Dictionary<string, SoulCardScriptableObject> allCards,
        Dictionary<string, CardTemplate> addedCards)
    {
        // Log.Debug($"=== Post processing requirements for {addedCards.Count} cards ===");

        var addedCardNames = addedCards.Keys;
        foreach (var cardName in addedCardNames)
        {
            // Log.Debug($"Processing {cardName}");
            var cardTemplate = addedCards[cardName];
            var cardScso = allCards[cardName];

            if (cardTemplate.RequiresAny != null)
            {
                // Log.Debug($"\t{cardName} - RequiresAny");
                cardScso.CardRequirement = cardTemplate.RequiresAny.ToRequirementList();
            }

            if (cardTemplate.RequiresAll != null)
            {
                // Log.Debug($"\t{cardName} - RequiresAll");
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
            SoulCardScriptableObject cardScso;
            if (allCards.TryGetValue(cardToGet, out cardScso))
            {
                if (cardScso != null)
                {
                    result.Add(cardScso);
                }
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
                Log.Warn($"Could not convert stat: {statName} ");
            }
        }

        return result.ToArray();
    }
}
