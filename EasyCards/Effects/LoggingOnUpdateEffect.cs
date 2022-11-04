namespace EasyCards.Effects;

using System;
using Il2CppInterop.Runtime.Injection;
using RogueGenesia.Actors.Survival;

public class LoggingOnUpdateEffect : AbstractEffect, IOnUpdate
{
    public LoggingOnUpdateEffect(IntPtr ptr) : base(ptr) { }
    public LoggingOnUpdateEffect() : base(ClassInjector.DerivedConstructorPointer<LoggingOnUpdateEffect>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public void OnUpdate(PlayerEntity owner)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnUpdate");
    }

    public override float Duration() => 10;
}
