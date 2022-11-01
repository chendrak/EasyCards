namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using RogueGenesia.Data;
using RogueGenesia.GameManager;

public abstract class NumberOfCardSelectionsOnAcquireCard : SingleEffectCard
{
    protected virtual int NumberOfCards => throw new NotImplementedException();
    protected override CardTag Tags => CardTag.None;
    protected override int MaxLevel => 1;
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;
    public override Dictionary<string, string> LocalizedDescriptions => new()
    {
        ["en"] = $"Grants you {this.NumberOfCards} cards",
    };

    protected override void OnAcquire(PlayerData playerData)
    {
        for (int i = 0; i < NumberOfCards; i++)
        {
            GameManagerFight.instance.OnOpenLevelUp();
        }
    }

    protected NumberOfCardSelectionsOnAcquireCard(IntPtr ptr) : base(ptr) {}
}
