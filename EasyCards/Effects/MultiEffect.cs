namespace EasyCards.Effects;

using System;
using System.Collections.Generic;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public abstract class MultiEffect : AbstractEffect, IOnUpdate, IOnDash, IOnKill
{
    public MultiEffect(IntPtr ptr) : base(ptr)
    {
        this.InitializeEffects();
    }

    private List<AbstractEffect> Effects { get; set; } = new();

    protected virtual void InitializeEffects() => throw new NotImplementedException();

    protected void AddEffect(AbstractEffect effect)
    {
        this.Effects.Add(effect);
    }

    public void OnUpdate(PlayerEntity owner)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnUpdate");
        foreach (var effect in this.Effects)
        {
            if (effect is IOnUpdate onUpdateEffect)
            {
                onUpdateEffect.OnUpdate(owner);
            }
        }
    }

    public void OnKill(PlayerData owner, IEntity killedEntity)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnKill");
        foreach (var effect in this.Effects)
        {
            if (effect is IOnKill onKillEffect)
            {
                onKillEffect.OnKill(owner, killedEntity);
            }
        }
    }

    public void OnDash(PlayerEntity owner)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnDash");
        foreach (var effect in this.Effects)
        {
            if (effect is IOnDash onDashEffect)
            {
                onDashEffect.OnDash(owner);
            }
        }
    }
}
