using EasyCards.Services;
using RogueGenesia.Data;
using UnityEngine.InputSystem;

namespace EasyCards.Bootstrap;

using BepInEx.Logging;
using Logging;

public sealed class DebugHelper : IDebugHelper, IInputEventSubscriber
{
    private readonly ILoggerConfiguration loggerConfiguration;
    private readonly ICardRepository cardRepository;
    private readonly ManualLogSource Logger = EasyCards.Instance.Log;

    public DebugHelper(ILoggerConfiguration loggerConfiguration, ICardRepository cardRepository)
    {
        this.loggerConfiguration = loggerConfiguration;
        this.cardRepository = cardRepository;
        Logger = EasyCards.Instance.Log;
    }

    public void Initialize()
    {
    }

    private void LogCard(SoulCardScriptableObject card)
    {
        Logger.LogDebug($"=== Card: {card.name} / Localized Name: {card.GetLocalizedName()} ===");

        var cardsToRemove = card.CardRemoved;
        Logger.LogDebug($"Removes cards: {cardsToRemove.Count}");
        foreach (var cardToRemove in cardsToRemove)
        {
            Logger.LogDebug($"\t{cardToRemove.name}");
        }

        var cardsToBanish = card.CardExclusion;
        Logger.LogDebug($"Banishes cards: {cardsToBanish.Count}");
        foreach (var cardToBanish in cardsToBanish)
        {
            Logger.LogDebug($"\t{cardToBanish.name}");
        }

        var CardExclusionString = card.CardExclusionString;
        Logger.LogDebug($"CardExclusionString: {CardExclusionString.Count}");
        foreach (var cardRef in CardExclusionString)
        {
            Logger.LogDebug($"\t{cardRef}");
        }

        var CardToRemoveString = card.CardToRemoveString;
        Logger.LogDebug($"CardToRemoveString: {CardToRemoveString.Count}");
        foreach (var cardRef in CardToRemoveString)
        {
            Logger.LogDebug($"\t{cardRef}");
        }

        var CardWithStatsToBan = card.CardWithStatsToBan;
        Logger.LogDebug($"CardWithStatsToBan: {CardWithStatsToBan.Count}");
        foreach (var cardRef in CardWithStatsToBan)
        {
            Logger.LogDebug($"\t{cardRef}");
        }

        var requiresAnyCard = card.CardRequirement;
        Logger.LogDebug($"Requires ANY of the following cards:");
        LogRequirements(requiresAnyCard);

        var requiresAllCard = card.HardCardRequirement;
        Logger.LogDebug($"Requires ALL of the following:");
        LogRequirements(requiresAllCard);
    }

    private void OnDebugLogKeyPressed()
    {
        if (!this.loggerConfiguration.IsLoggerEnabled())
            return;

        Logger.LogDebug("OnDebugLogKeyPressed");
        var allCards = this.cardRepository.GetAllCards();

        Logger.LogDebug($"=== Listing All Cards ({allCards.Length}) ===");

        foreach (var card in allCards)
        {
            LogCard(card);
        }

        var excludedCards = SoulCardScriptableObject.GetExcludedSoulCard();

        Logger.LogDebug($"=== Listing All Currently Excluded Cards ({excludedCards.Count}) ===");

        foreach (var excludedCard in excludedCards)
        {
            Logger.LogDebug($"\t{excludedCard.name}");
        }

        var player = GameData.PlayerDatabase[0];

        var heldSoulCards = player._soulCardList;
        Logger.LogDebug($"=== Listing All Currently Held Soul Cards [{heldSoulCards.Count}] ===");

        foreach (var heldSoulCard in heldSoulCards)
        {
            Logger.LogDebug($"\t{heldSoulCard._name}");
        }

        var heldSoulCardSoList = player._soulCardSOList;
        Logger.LogDebug($"=== Listing All Currently Held Soul Card Scriptable Objects [{heldSoulCardSoList.Count}] ===");

        foreach (var heldSoulCard in heldSoulCardSoList)
        {
            Logger.LogDebug($"\t{heldSoulCard.name}");
        }
    }


    public bool Enabled => true;
    public void LogRequirements(SCSORequirementList requirementList, string prefix = "\t")
    {
        if (!this.loggerConfiguration.IsLoggerEnabled())
            return;

        if (requirementList == null)
            return;

        var cardRequirements = requirementList.CardRequirement;
        if (cardRequirements is { Count: > 0 })
        {
            Logger.LogDebug($"{prefix}Cards:");
            foreach (var cardRequirement in cardRequirements)
            {
                Logger.LogDebug($"{prefix}{prefix}{cardRequirement.RequiredCard.name}, Lvl {cardRequirement.RequiredCardLevel}");
            }
        }

        var statReqs = requirementList.StatsRequirement;
        if (statReqs is { Count: > 0 })
        {
            Logger.LogDebug($"{prefix}Stats:");
            foreach (var statReq in statReqs)
            {
                Logger.LogDebug($"{prefix}{prefix}Modifiers:");
                foreach (var statModifier in statReq.RequiredStats.ModifiersList)
                {
                    Logger.LogDebug(
                        $"{prefix}{prefix}{prefix}{statModifier.Key}: {statModifier.Value.Value} ({statModifier.Value.ModifierType})");
                }

                var typeString = statReq.RequireMore ? "Min" : "Max";

                Logger.LogDebug($"{prefix}{prefix}{prefix}Type: {typeString}");
            }
        }
    }

    public bool HandlesKey(Key key) => key == Key.L;

    public void OnInputEvent(Key key)
    {
        OnDebugLogKeyPressed();
    }
}
