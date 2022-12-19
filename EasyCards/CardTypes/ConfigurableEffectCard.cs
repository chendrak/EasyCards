namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using Effects;
using Events;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ConfigurableEffectCard : SoulCard
{
    protected ManualLogSource Log = EasyCards.Instance.Log;

    public ConfigurableEffectCard(IntPtr ptr) : base(ptr) { }

    private List<ConfigurableEffect> Effects = new();

    // Trigger Lists
    private List<ConfigurableEffect> OnKillEffects { get; set; } = new();
    private List<ConfigurableEffect> OnDashEffects { get; set; } = new();
    private List<ConfigurableEffect> OnStageStartEffects { get; set; } = new();
    private List<ConfigurableEffect> OnStageEndEffects { get; set; } = new();
    private List<ConfigurableEffect> OnDeathEffects { get; set; } = new();
    private List<ConfigurableEffect> OnTakeDamageEffects { get; set; } = new();

    // Type Lists
    private List<ConfigurableEffect> DurationEffects { get; set; } = new();
    private List<ConfigurableEffect> IntervalEffects { get; set; } = new();

    private float levelStartTime;

    private void InitializeEffects(string name)
    {
        this.Log.LogInfo($"InitializeEffects({name})");
        this.Effects = EffectHolder.GetEffects(name);
        this.Log.LogInfo($"Number of effects: {this.Effects.Count}");

        this.OnKillEffects = this.Effects.Where(effect =>
            effect.Trigger is EffectTrigger.OnKill or EffectTrigger.OnEliteKill or EffectTrigger.OnBossKill ||
            effect.ActivationRequirement == EffectActivationRequirement.EnemiesKilled).ToList();

        this.OnDashEffects = this.Effects.Where(effect => effect.Trigger == EffectTrigger.OnDash).ToList();
        this.OnStageStartEffects = this.Effects.Where(effect => effect.Trigger == EffectTrigger.OnStageStart).ToList();
        this.OnStageEndEffects = this.Effects.Where(effect => effect.Trigger == EffectTrigger.OnStageEnd).ToList();
        this.OnDeathEffects = this.Effects.Where(effect => effect.Trigger == EffectTrigger.OnDeath).ToList();
        this.OnTakeDamageEffects = this.Effects.Where(effect =>
            effect.Trigger == EffectTrigger.OnTakeDamage ||
            effect.ActivationRequirement == EffectActivationRequirement.DamageTaken).ToList();

        this.DurationEffects = this.Effects.Where(effect => effect.Type == EffectType.Duration).ToList();
        this.IntervalEffects = this.Effects.Where(effect => effect.Type == EffectType.Interval).ToList();

        foreach (var effect in this.Effects)
        {
            if (effect.ActivationRequirement == EffectActivationRequirement.None)
            {
                effect.Enable(Time.time);
            }
            else if (effect.ActivationRequirement == EffectActivationRequirement.StageStart &&
                     GameData.GameState is EGameState.RoguePlay or EGameState.Survivor)
            {
                // We're in Rogs Mode in a fight level or in Survivors Mode, activate immediately
                effect.Enable(Time.time);
            }
        }
    }

    public override void Init(AvatarData owner)
    {
        this.InitializeEffects(this._name);

        GameEvents.OnRunEndEvent += this.CleanUpEvents;
        GameEvents.OnPlayerFinalDeathEvent += this.CleanUpEvents;

        GameEvents.OnRogueLevelStartedEvent += this.OnRogueLevelStarted;
        GameEvents.OnRogueLevelEndedEvent += this.OnRogueLevelEnded;
        GameEvents.OnDeathEvent += this.OnDeath;

        GameEvents.OnPlayerTakeDamageEvent += this.OnPlayerTakeDamage;
    }

    private void OnPlayerTakeDamage(DamageInformation damageInfo)
    {
        this.Log.LogInfo($"{this._name}.OnTakeDamage");
        foreach (var takeDamageEffect in this.OnTakeDamageEffects)
        {
            takeDamageEffect.OnTakeDamage(damageInfo.DamageValue);
        }
    }

    private void OnDeath()
    {
        this.Log.LogInfo($"OnDeath()");
        foreach (var deathEffect in this.OnDeathEffects)
        {
            deathEffect.Apply();
        }
    }

    private void OnRogueLevelStarted()
    {
        this.levelStartTime = Time.time;
        foreach (var effect in this.Effects.Where(effect =>
                     effect.ActivationRequirement == EffectActivationRequirement.StageStart))
        {
            effect.Enable(this.levelStartTime);
        }
    }

    private void OnRogueLevelEnded() { }

    public override void OnUpdate(PlayerEntity owner)
    {
        var currentTime = Time.time;

        foreach (var durationEffect in this.DurationEffects)
        {
            durationEffect.OnUpdate(owner, currentTime);
        }

        foreach (var intervalEffect in this.IntervalEffects)
        {
            intervalEffect.OnUpdate(owner, currentTime);
        }
    }

    public override void OnDash(PlayerEntity owner)
    {
        this.Log.LogInfo($"OnDash()");
        foreach (var effect in this.OnDashEffects)
        {
            effect.Apply();
        }
    }

    public override void OnKill(AvatarData owner, IEntity killedEntity)
    {
        var collisionDetection = killedEntity.GetLinkedGameObject().GetComponent<MonsterCollisionDetection>();
        if (collisionDetection != null)
        {
            var monster = collisionDetection.LinkedMonster;
            foreach (var onKillEffect in this.OnKillEffects)
            {
                onKillEffect.OnKill(monster);
            }
        }
    }


    private void CleanUpEvents()
    {
        this.Log.LogInfo($"CleanUpEvents");
        GameEvents.OnRunEndEvent -= this.CleanUpEvents;
        GameEvents.OnPlayerFinalDeathEvent -= this.CleanUpEvents;

        GameEvents.OnRogueLevelStartedEvent -= this.OnRogueLevelStarted;
        GameEvents.OnRogueLevelEndedEvent -= this.OnRogueLevelEnded;

        GameEvents.OnPlayerTakeDamageEvent -= this.OnPlayerTakeDamage;
    }
}
