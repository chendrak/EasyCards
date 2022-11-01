namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using RogueGenesia.Data;
using RogueGenesia.GameManager;

public abstract class RerollsOnAcquireCard : SingleEffectCard
{
    protected virtual int NumberOfRerolls => throw new NotImplementedException();
    protected override CardTag Tags => CardTag.None;
    protected override int MaxLevel => 1;
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;
    public override Dictionary<string, string> LocalizedDescriptions => new()
    {
        ["en"] = $"Grants you {this.NumberOfRerolls} rerolls",
    };

    protected override void OnAcquire(PlayerData playerData) => playerData.RerollLeft += this.NumberOfRerolls;

    protected RerollsOnAcquireCard(IntPtr ptr) : base(ptr) {}
}
