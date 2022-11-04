namespace EasyCards.Effects;

using System;
using Il2CppInterop.Runtime.Injection;

public class LoggingMultiEffect : MultiEffect
{
    public LoggingMultiEffect(IntPtr ptr) : base(ptr) { }
    public LoggingMultiEffect() : base(ClassInjector.DerivedConstructorPointer<LoggingMultiEffect>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    protected override void InitializeEffects()
    {
        this.AddEffect(new LoggingOnDashEffect());
        this.AddEffect(new LoggingOnKillEffect());
    }

    public override float Duration() => 10;
}
