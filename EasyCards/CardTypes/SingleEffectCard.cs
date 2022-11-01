namespace EasyCards.CardTypes;

using System;
using RogueGenesia.Data;

public abstract class SingleEffectCard : CustomSoulCard
{
    protected SingleEffectCard(IntPtr ptr) : base(ptr) {}
    protected virtual void OnAcquire(PlayerData playerData) => throw new NotImplementedException();

    public override void Init(PlayerData owner)
    {
        Log.LogInfo($"Init({owner})");
        this.OnAcquire(owner);
    }
}
