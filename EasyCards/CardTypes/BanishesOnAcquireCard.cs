namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using RogueGenesia.Data;

public abstract class BanishesOnAcquireCard : SingleEffectCard
{
    protected virtual int CardValue => throw new NotImplementedException();
    protected override CardTag Tags => CardTag.None;
    protected override int MaxLevel => 1;
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;
    public override Dictionary<string, string> LocalizedDescriptions => new()
    {
        ["en"] = $"Grants you {this.CardValue} banishes",
    };

    protected override void OnAcquire(PlayerData playerData) => playerData.BanishLeft += this.CardValue;

    protected BanishesOnAcquireCard(IntPtr ptr) : base(ptr) {}
}
