namespace EasyCards.Bootstrap;

using Common.Logging;
using Helpers;
using RogueGenesia.Data;
using Services;
using UnityEngine.InputSystem;

public static class DebugHelper
{
    public static void Initialize()
    {
        KeyPressHelper.RegisterKey(Key.L, Modifiers.Ctrl | Modifiers.Shift, OnDebugLogKeyPressed);
    }

    private static void LogCard(SoulCardScriptableObject card)
    {
        Log.Info($"=== Card: {card.name} / Localized Name: {card.GetLocalizedName()} ===");

        var cardsToRemove = card.CardRemoved;
        Log.Info($"Removes cards: {cardsToRemove.Length}");
        foreach (var cardToRemove in cardsToRemove)
        {
            Log.Info($"\t{cardToRemove.name}");
        }

        var cardsToBanish = card.CardExclusion;
        Log.Info($"Banishes cards: {cardsToBanish.Length}");
        foreach (var cardToBanish in cardsToBanish)
        {
            Log.Info($"\t{cardToBanish.name}");
        }

        var CardExclusionString = card.CardExclusionString;
        Log.Info($"CardExclusionString: {CardExclusionString.Length}");
        foreach (var cardRef in CardExclusionString)
        {
            Log.Info($"\t{cardRef}");
        }

        var CardToRemoveString = card.CardToRemoveString;
        Log.Info($"CardToRemoveString: {CardToRemoveString.Length}");
        foreach (var cardRef in CardToRemoveString)
        {
            Log.Info($"\t{cardRef}");
        }

        var CardWithStatsToBan = card.CardWithStatsToBan;
        Log.Info($"CardWithStatsToBan: {CardWithStatsToBan.Length}");
        foreach (var cardRef in CardWithStatsToBan)
        {
            Log.Info($"\t{cardRef}");
        }

        var requiresAnyCard = card.CardRequirement;
        Log.Info($"Requires ANY of the following cards:");
        LogRequirements(requiresAnyCard);

        var requiresAllCard = card.HardCardRequirement;
        Log.Info($"Requires ALL of the following:");
        LogRequirements(requiresAllCard);
    }

    private static void OnDebugLogKeyPressed()
    {
        Log.Info("OnDebugLogKeyPressed");
        var allCards = CardRepository.GetAllCards();

        Log.Info($"=== Listing All Cards ({allCards.Count}) ===");

        foreach (var card in allCards)
        {
            LogCard(card);
        }

        if (GameData.GameState is EGameState.MainMenu or EGameState.GameOver)
            return;

        var excludedCards = SoulCardScriptableObject.GetExcludedSoulCard();

        Log.Info($"=== Listing All Currently Excluded Cards ({excludedCards.Count}) ===");

        foreach (var excludedCard in excludedCards)
        {
            Log.Info($"\t{excludedCard.name}");
        }

        var player = GameData.PlayerDatabase[0];

        var heldSoulCards = player._soulCardList;
        Log.Info($"=== Listing All Currently Held Soul Cards [{heldSoulCards.Count}] ===");

        foreach (var heldSoulCard in heldSoulCards)
        {
            Log.Info($"\t{heldSoulCard.GetName}");
        }

        var heldSoulCardSoList = player._soulCardSOList;
        Log.Info($"=== Listing All Currently Held Soul Card Scriptable Objects [{heldSoulCardSoList.Count}] ===");

        foreach (var heldSoulCard in heldSoulCardSoList)
        {
            Log.Info($"\t{heldSoulCard.name}");
        }
    }

    internal static void LogRequirements(SCSORequirementList requirementList, string prefix = "\t")
    {
        // if (requirementList == null)
        //     return;
        //
        // var cardRequirements = requirementList.CardRequirement;
        // if (cardRequirements is { Count: > 0 })
        // {
        //     Log.Info($"{prefix}Cards:");
        //     foreach (var cardRequirement in cardRequirements)
        //     {
        //         Log.Info($"{prefix}{prefix}{cardRequirement.RequiredCard.name}, Lvl {cardRequirement.RequiredCardLevel}");
        //     }
        // }
        //
        // var statReqs = requirementList.StatsRequirement;
        // if (statReqs is { Count: > 0 })
        // {
        //     Log.Info($"{prefix}Stats:");
        //     foreach (var statReq in statReqs)
        //     {
        //         Log.Info($"{prefix}{prefix}Modifiers:");
        //         foreach (var statModifier in statReq.RequiredStats.ModifiersList)
        //         {
        //             Log.Info(
        //                 $"{prefix}{prefix}{prefix}{statModifier.Key}: {statModifier.Value.Value} ({statModifier.Value.ModifierType})");
        //         }
        //
        //         var typeString = statReq.ComparisionType == StatRequirement.EComparisionType.LesserOrEqual ? "Min" : "Max";
        //
        //         Log.Info($"{prefix}{prefix}{prefix}Type: {typeString}");
        //     }
        // }
    }
}
