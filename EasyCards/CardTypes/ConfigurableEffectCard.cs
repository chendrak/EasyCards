namespace EasyCards.CardTypes;

using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Effects;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using UnityEngine;

public class ConfigurableEffectCard : SoulCard
{
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
        Log.Info($"InitializeEffects({name})");
        this.Effects = EffectHolder.GetEffects(name);
        Log.Info($"Number of effects: {this.Effects.Count}");

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

        GameEventManager.OnStageStart.AddListener(this.OnRogueLevelStarted);
        GameEventManager.OnStageEnd.AddListener(this.OnRogueLevelEnded);
        GameEventManager.OnPlayerDeath.AddListener(this.OnDeath);
        GameEventManager.OnPlayerTakeDamage_BeforeApplying.AddListener(this.OnPlayerTakeDamage);
    }

    private void OnPlayerTakeDamage(PlayerEntity playerEntity, AvatarData avatarData, DamageInformation damageInformation)
    {
        Log.Info($"{this._name}.OnTakeDamage");
        foreach (var takeDamageEffect in this.OnTakeDamageEffects)
        {
            takeDamageEffect.OnTakeDamage(damageInformation.DamageValue);
        }
    }

    private void OnDeath(PlayerEntity playerEntity, AvatarData avatarData)
    {
        Log.Info($"OnDeath()");
        foreach (var deathEffect in this.OnDeathEffects)
        {
            deathEffect.Apply();
        }
    }

    private void OnRogueLevelStarted(LevelObject level)
    {
        this.levelStartTime = Time.time;
        foreach (var effect in this.Effects.Where(effect =>
                     effect.ActivationRequirement == EffectActivationRequirement.StageStart))
        {
            effect.Enable(this.levelStartTime);
        }
    }

    private void OnRogueLevelEnded(LevelObject level) { }

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
        Log.Info($"OnDash()");
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
}
