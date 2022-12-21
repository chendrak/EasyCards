namespace EasyCards.Bootstrap;

using RogueGenesia.Data;
using UnityEngine.InputSystem;
using BepInEx.Logging;
using Common.Helpers;
using Services;

public static class DebugHelper
{
    private static readonly ManualLogSource Logger = EasyCards.Instance.Log;

    public static void Initialize()
    {
        KeyPressHelper.RegisterKey(Key.L, Modifiers.Ctrl | Modifiers.Shift, OnDebugLogKeyPressed);
    }

    private static void LogCard(SoulCardScriptableObject card)
    {
        Logger.LogInfo($"=== Card: {card.name} / Localized Name: {card.GetLocalizedName()} ===");

        var cardsToRemove = card.CardRemoved;
        Logger.LogInfo($"Removes cards: {cardsToRemove.Count}");
        foreach (var cardToRemove in cardsToRemove)
        {
            Logger.LogInfo($"\t{cardToRemove.name}");
        }

        var cardsToBanish = card.CardExclusion;
        Logger.LogInfo($"Banishes cards: {cardsToBanish.Count}");
        foreach (var cardToBanish in cardsToBanish)
        {
            Logger.LogInfo($"\t{cardToBanish.name}");
        }

        var CardExclusionString = card.CardExclusionString;
        Logger.LogInfo($"CardExclusionString: {CardExclusionString.Count}");
        foreach (var cardRef in CardExclusionString)
        {
            Logger.LogInfo($"\t{cardRef}");
        }

        var CardToRemoveString = card.CardToRemoveString;
        Logger.LogInfo($"CardToRemoveString: {CardToRemoveString.Count}");
        foreach (var cardRef in CardToRemoveString)
        {
            Logger.LogInfo($"\t{cardRef}");
        }

        var CardWithStatsToBan = card.CardWithStatsToBan;
        Logger.LogInfo($"CardWithStatsToBan: {CardWithStatsToBan.Count}");
        foreach (var cardRef in CardWithStatsToBan)
        {
            Logger.LogInfo($"\t{cardRef}");
        }

        var requiresAnyCard = card.CardRequirement;
        Logger.LogInfo($"Requires ANY of the following cards:");
        LogRequirements(requiresAnyCard);

        var requiresAllCard = card.HardCardRequirement;
        Logger.LogInfo($"Requires ALL of the following:");
        LogRequirements(requiresAllCard);
    }

    private static void OnDebugLogKeyPressed()
    {
        Logger.LogInfo("OnDebugLogKeyPressed");
        var allCards = CardRepository.GetAllCards();

        Logger.LogInfo($"=== Listing All Cards ({allCards.Length}) ===");

        foreach (var card in allCards)
        {
            LogCard(card);
        }

        if (GameData.GameState is EGameState.MainMenu or EGameState.GameEnd)
            return;

        var excludedCards = SoulCardScriptableObject.GetExcludedSoulCard();

        Logger.LogInfo($"=== Listing All Currently Excluded Cards ({excludedCards.Count}) ===");

        foreach (var excludedCard in excludedCards)
        {
            Logger.LogInfo($"\t{excludedCard.name}");
        }

        var player = GameData.PlayerDatabase[0];

        var heldSoulCards = player._soulCardList;
        Logger.LogInfo($"=== Listing All Currently Held Soul Cards [{heldSoulCards.Count}] ===");

        foreach (var heldSoulCard in heldSoulCards)
        {
            Logger.LogInfo($"\t{heldSoulCard._name}");
        }

        var heldSoulCardSoList = player._soulCardSOList;
        Logger.LogInfo($"=== Listing All Currently Held Soul Card Scriptable Objects [{heldSoulCardSoList.Count}] ===");

        foreach (var heldSoulCard in heldSoulCardSoList)
        {
            Logger.LogInfo($"\t{heldSoulCard.name}");
        }
    }

    internal static void LogRequirements(SCSORequirementList requirementList, string prefix = "\t")
    {
        if (requirementList == null)
            return;

        var cardRequirements = requirementList.CardRequirement;
        if (cardRequirements is { Count: > 0 })
        {
            Logger.LogInfo($"{prefix}Cards:");
            foreach (var cardRequirement in cardRequirements)
            {
                Logger.LogInfo($"{prefix}{prefix}{cardRequirement.RequiredCard.name}, Lvl {cardRequirement.RequiredCardLevel}");
            }
        }

        var statReqs = requirementList.StatsRequirement;
        if (statReqs is { Count: > 0 })
        {
            Logger.LogInfo($"{prefix}Stats:");
            foreach (var statReq in statReqs)
            {
                Logger.LogInfo($"{prefix}{prefix}Modifiers:");
                foreach (var statModifier in statReq.RequiredStats.ModifiersList)
                {
                    Logger.LogInfo(
                        $"{prefix}{prefix}{prefix}{statModifier.Key}: {statModifier.Value.Value} ({statModifier.Value.ModifierType})");
                }

                var typeString = statReq.RequireMore ? "Min" : "Max";

                Logger.LogInfo($"{prefix}{prefix}{prefix}Type: {typeString}");
            }
        }
    }
}
