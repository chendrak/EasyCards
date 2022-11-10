namespace EasyCards.QoL;

using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using HarmonyLib;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using RogueGenesia.UI;
using Enum = System.Enum;

[HarmonyPatch]
public static class CardManagerHooks
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.UpdateStats))]
    public static void PostStatsUpdate()
    {
        var stats = GameData.PlayerDatabase[0]._playerStats;
        CardManager.OnPlayerStatsUpdated(stats);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnStartNewGame))]
    public static void PostOnStartNewGame()
    {
        var cards = GameData.GetAllCards().ToList();
        var stats = GameData.PlayerDatabase[0]._playerStats;
        CardManager.Initialize(cards, stats);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UILevelUp), nameof(UILevelUp.OnBanish))]
    public static void PostOnBanish(SoulCardScriptableObject SCUI) => CardManager.OnCardBanished(SCUI);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManagerFight), nameof(GameManagerFight.OnSelectLevelUp))]
    public static void PostOnCardSelect(SoulCardScriptableObject SelectedBonus) => CardManager.OnCardSelected(SelectedBonus);
}

public static class CardManager
{
    private static PlayerStats LatestPlayerStats;
    private static List<SoulCardScriptableObject> AllCards = new();
    private static List<SoulCardScriptableObject> AllWeapons = new();
    private static Dictionary<string, SoulCardScriptableObject> CardsAvailableToThePlayer = new();
    private static Dictionary<string, SoulCardScriptableObject> WeaponsAvailableToThePlayer = new();
    private static Dictionary<string, SoulCardScriptableObject> BanishedCards = new();
    private static Dictionary<string, SoulCardScriptableObject> ExcludedCards = new();
    private static Dictionary<string, SoulCardScriptableObject> CardsByName = new();
    private static Dictionary<StatsType, List<SoulCardScriptableObject>> CardsByStats = new();
    private static Dictionary<string, SoulCardScriptableObject> CurrentlyHeldCards = new();

    private static ManualLogSource Log = EasyCards.Instance.Log;


    public static void Initialize(List<SoulCardScriptableObject> cards, PlayerStats stats)
    {
        LatestPlayerStats = stats;

        AllCards = new List<SoulCardScriptableObject>();
        foreach (var card in cards)
        {
            AddCard(card);
        }

        LogState($"Initialize()");
        OnPlayerStatsUpdated(stats);
    }

    public static void AddCard(SoulCardScriptableObject card)
    {
        AllCards.Add(card);
        if (card.SoulCardType == CardType.Weapon)
        {
            AllWeapons.Add(card);
        }

        if (!card.HasCardRequirements())
        {
            CardsAvailableToThePlayer[card.name] = card;
            if (card.SoulCardType == CardType.Weapon)
            {
                WeaponsAvailableToThePlayer[card.name] = card;
            }
        }
        else
        {
            var isSoftRequirementFulfilled = card.CardRequirement?.IsRequirementFullfilled(false) ?? true;
            var isHardRequirementFulfilled = card.HardCardRequirement?.IsRequirementFullfilled(true) ?? true;

            if (!isSoftRequirementFulfilled && !isHardRequirementFulfilled)
            {
                ExcludedCards[card.name] = card;
            }
        }

        CardsByName[card.name] = card;
        foreach (var modifier in card.StatsModifier.ModifiersList)
        {
            StatsType parsedStatType;
            if (Enum.TryParse(modifier.Key, true, out parsedStatType))
            {
                List<SoulCardScriptableObject> cards;
                if (!CardsByStats.TryGetValue(parsedStatType, out cards))
                {
                    cards = new();
                    CardsByStats[parsedStatType] = cards;
                }

                cards.Add(card);
            }
        }
    }

    public static void OnCardBanished(SoulCardScriptableObject card)
    {
        BanishedCards[card.name] = card;
        CardsAvailableToThePlayer.Remove(card.name);

        LogState($"OnCardBanished({card.name})");
    }

    public static void OnCardSelected(SoulCardScriptableObject card)
    {
        // Remove cards that this card removes
        foreach (var cardName in card.CardToRemoveString)
        {
            CurrentlyHeldCards.Remove(cardName);
        }

        CurrentlyHeldCards[card.name] = card;

        foreach (var cardName in card.CardExclusionString)
        {
            ExcludedCards[cardName] = card;
            CardsAvailableToThePlayer.Remove(cardName);
        }

        foreach (var statsType in card.CardWithStatsToBan)
        {
            List<SoulCardScriptableObject> cardsForStat;
            if (CardsByStats.TryGetValue(statsType, out cardsForStat))
            {
                foreach (var cardForStat in cardsForStat)
                {
                    ExcludedCards[cardForStat.name] = cardForStat;
                }
            }
        }

        LogState($"OnCardSelected({card.name})");
    }

    public static void OnPlayerStatsUpdated(PlayerStats stats)
    {
        LatestPlayerStats = stats;
        foreach (var card in AllCards)
        {
            if (ExcludedCards.ContainsKey(card.name)) continue;
            if (BanishedCards.ContainsKey(card.name)) continue;

            var isSoftRequirementFulfilled = card.CardRequirement?.IsRequirementFullfilled(false) ?? true;
            var isHardRequirementFulfilled = card.HardCardRequirement?.IsRequirementFullfilled(true) ?? true;

            if (CardsAvailableToThePlayer.ContainsKey(card.name))
            {
                // Check for removal
                if (!isSoftRequirementFulfilled && !isHardRequirementFulfilled)
                {
                    CardsAvailableToThePlayer.Remove(card.name);
                    if (card.IsWeapon()) WeaponsAvailableToThePlayer.Remove(card.name);
                    continue;
                }
            }

            if (isHardRequirementFulfilled || isSoftRequirementFulfilled)
            {
                CardsAvailableToThePlayer[card.name] = card;
                if (card.IsWeapon()) WeaponsAvailableToThePlayer[card.name] = card;
            }
        }

        LogState("OnPlayerStatsUpdated");
    }

    private static void LogState(string source)
    {
        Log.LogInfo($"Logging stats from {source}");
        var pd = GameData.PlayerDatabase[0];
        Log.LogInfo($"=== Held cards - CardManager: {CurrentlyHeldCards.Count} - Game: {pd._soulCardSOList.Count}");
        if (CurrentlyHeldCards.Count != pd._soulCardList.Count)
        {
            Log.LogInfo($"They are not equal, listing them out.");
            Log.LogInfo($"CardManager:");
            foreach (var cardName in CurrentlyHeldCards.Keys)
            {
                Log.LogInfo(cardName);
            }
            Log.LogInfo($"Game:");
            foreach (var card in pd._soulCardSOList)
            {
                Log.LogInfo(card.name);
            }
        }

        var excludedCards = SoulCardScriptableObject.GetExcludedSoulCard();
        Log.LogInfo($"=== Excluded cards - CardManager: {ExcludedCards.Count} - Game: {excludedCards.Count}");
        if (ExcludedCards.Count != excludedCards.Count)
        {
            Log.LogInfo($"They are not equal, listing them out.");
            Log.LogInfo($"CardManager:");
            foreach (var cardName in ExcludedCards.Keys)
            {
                Log.LogInfo(cardName);
            }
            Log.LogInfo($"Game:");
            foreach (var card in excludedCards)
            {
                Log.LogInfo(card.name);
            }
        }

        var banishedCards = GameData.BanishedCard;
        Log.LogInfo($"=== Banished cards - CardManager: {BanishedCards.Count} - Game: {banishedCards.Count}");
        if (BanishedCards.Count != banishedCards.Count)
        {
            Log.LogInfo($"They are not equal, listing them out.");
            Log.LogInfo($"CardManager:");
            foreach (var cardName in BanishedCards.Keys)
            {
                Log.LogInfo(cardName);
            }
            Log.LogInfo($"Game:");
            foreach (var card in banishedCards)
            {
                Log.LogInfo(card.name);
            }
        }
    }

    private static bool HasCardRequirements(this SoulCardScriptableObject card)
    {
        var hasSoftRequirements = card.CardRequirement?.HasCardRequirements() ?? false;
        var hasHardRequirements = card.HardCardRequirement?.HasCardRequirements() ?? false;

        return hasSoftRequirements || hasHardRequirements;
    }

    private static bool HasCardRequirements(this SCSORequirementList requirementList)
    {
        return requirementList.CardRequirement is { Count: > 0 };
    }

    private static bool IsWeapon(this SoulCardScriptableObject card) => card.SoulCardType == CardType.Weapon;
}
