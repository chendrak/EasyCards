namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using Effects;
using Events;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public abstract class EffectCard : CustomSoulCard
{
    public EffectCard(IntPtr ptr) : base(ptr)
    {
        this.InitializeEffects();
    }

    private List<AbstractEffect> Effects = new();
    private Queue<AbstractEffect> QueuedEffects { get; set; } = new();

    public AbstractEffect? ActiveEffect { get; private set; }
    protected bool IsCardActive { get; set; }

    private float levelStartTime;
    private float activeEffectStartTime;

    protected virtual void InitializeEffects() => throw new NotImplementedException();

    protected void AddEffect(AbstractEffect effect)
    {
        this.Effects.Add(effect);
    }

    public override void Init(PlayerData owner)
    {
        this.Log.LogInfo($"Init({owner}) - Number of effects: {this.Effects.Count}");

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

    private void ResetEffectsToInitialState()
    {
        this.QueuedEffects = new Queue<AbstractEffect>(this.Effects);
    }

    private void EnableCard()
    {
        this.levelStartTime = Time.time;
        this.ResetEffectsToInitialState();
        this.ActivateNextEffect();
        this.IsCardActive = true;
    }

    private void ActivateNextEffect()
    {
        if (this.ActiveEffect != null)
        {
            this.QueuedEffects.Enqueue(this.ActiveEffect);
        }

        this.ActiveEffect = this.QueuedEffects.Dequeue();
        this.activeEffectStartTime = Time.time;

        this.Log.LogInfo($"Activated {this.ActiveEffect.GetType().Name}");
    }

    private void OnRogueLevelStarted() => this.EnableCard();

    private void OnRogueLevelEnded() => this.IsCardActive = false;

    public override void OnUpdate(PlayerEntity owner)
    {
        if (!this.IsCardActive)
        {
            return;
        }

        var currentTime = Time.time;

        if (this.ActiveEffect.HasLimitedDuration() && this.activeEffectStartTime + this.ActiveEffect.Duration() < currentTime)
        {
            this.ActivateNextEffect();
        }

        if (this.ActiveEffect is IOnUpdate effect)
        {
            effect.OnUpdate(owner);
        }
    }

    public override void OnDash(PlayerEntity owner)
    {
        if (this.ActiveEffect is IOnDash effect)
        {
            effect.OnDash(owner);
        }
    }

    public override void OnKill(PlayerData owner, IEntity killedEntity)
    {
        if (this.ActiveEffect is IOnKill effect)
        {
            effect.OnKill(owner, killedEntity);
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
