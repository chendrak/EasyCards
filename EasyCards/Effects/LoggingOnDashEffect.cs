namespace EasyCards.Effects;

using System;
using Il2CppInterop.Runtime.Injection;
using RogueGenesia.Actors.Survival;

public class LoggingOnDashEffect : AbstractEffect, IOnDash
{
    public LoggingOnDashEffect(IntPtr ptr) : base(ptr) { }
    public LoggingOnDashEffect() : base(ClassInjector.DerivedConstructorPointer<LoggingOnDashEffect>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }


    public void OnDash(PlayerEntity owner)
    {
        this.Log.LogInfo($"{this.GetType().Name}.OnDash");
    }

    public override float Duration() => 10;
}
