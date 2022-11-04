namespace EasyCards.Effects;

using System;
using BepInEx.Logging;
using Object = Il2CppSystem.Object;

public abstract class AbstractEffect : Object
{
    protected ManualLogSource Log => EasyCards.Instance.Log;

    public AbstractEffect(IntPtr ptr) : base(ptr) {}

    // public virtual void Activate() => throw new NotImplementedException();
    // public virtual bool IsExpired() => throw new NotImplementedException();

    public virtual float Duration() => throw new NotImplementedException();
    public bool HasLimitedDuration() => this.Duration() > 0;
}
