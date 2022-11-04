namespace EasyCards.Effects;

using System;
using Il2CppInterop.Runtime.Injection;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public class LoggingOnKillEffect : AbstractEffect, IOnKill
{
    public LoggingOnKillEffect(IntPtr ptr) : base(ptr) { }
    public LoggingOnKillEffect() : this(ClassInjector.DerivedConstructorPointer<LoggingOnKillEffect>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public void OnKill(PlayerData owner, IEntity killedEntity)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnKill");
    }

    public override float Duration() => 10;
}
