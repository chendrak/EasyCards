namespace EasyCards.CardTypes;

using System;
using Events;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public abstract class TimeLimitedEffectCard : CustomSoulCard
{
    protected TimeLimitedEffectCard(IntPtr ptr) : base(ptr) {}

    protected bool IsActive { get; set; }
    private float levelStartTime;
    protected virtual float TotalActiveTimeInSeconds => throw new NotImplementedException();

    public override void Init(PlayerData owner)
    {
        this.Log.LogInfo($"Init({owner})");

        GameEvents.OnGameEndEvent += this.CleanUpEvents;
        GameEvents.OnPlayerFinalDeathEvent += this.CleanUpEvents;

        GameEvents.OnRogueLevelStartedEvent += this.OnRogueLevelStarted;
        GameEvents.OnRogueLevelEndedEvent += this.OnRogueLevelEnded;

        if (GameData.GameState == EGameState.RoguePlay)
        {
            this.Log.LogInfo($"We're in a rogue stage, enabling {this.Name}");
            this.EnableCard();
        }
    }

    private void EnableCard()
    {
        this.levelStartTime = Time.time;
        this.IsActive = true;
    }

    private void OnRogueLevelStarted() => this.EnableCard();

    private void OnRogueLevelEnded() => this.IsActive = false;

    public override void OnUpdate(PlayerEntity owner)
    {
        if (!this.IsActive)
        {
            return;
        }

        var activeTime = Time.time - this.levelStartTime;
        if (activeTime > this.TotalActiveTimeInSeconds)
        {
            this.Log.LogInfo($"Time ran out, deactivating {this.Name}");
            this.IsActive = false;
        }
    }

    private void CleanUpEvents()
    {
        this.Log.LogInfo($"CleanUpEvents");
        GameEvents.OnGameEndEvent -= this.CleanUpEvents;
        GameEvents.OnPlayerFinalDeathEvent -= this.CleanUpEvents;

        GameEvents.OnRogueLevelStartedEvent -= this.OnRogueLevelStarted;
        GameEvents.OnRogueLevelEndedEvent -= this.OnRogueLevelEnded;
    }
}
