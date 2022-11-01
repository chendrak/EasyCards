namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using RogueGenesia.Data;

public abstract class LevelOnAcquireCard : SingleEffectCard
{
    protected virtual int NumberOfLevelUps => throw new NotImplementedException();
    protected override CardTag Tags => CardTag.None;
    protected override int MaxLevel => 1;
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;
    public override Dictionary<string, string> LocalizedDescriptions => new()
    {
        ["en"] = $"Grants you {this.NumberOfLevelUps} levels.",
    };

    protected override void OnAcquire(PlayerData playerData)
    {
        for (int i = 0; i < NumberOfLevelUps; i++)
        {
            playerData.PlayerEntity.GetSoulsLevel.LevelUp();
        }
    }

    protected LevelOnAcquireCard(IntPtr ptr) : base(ptr) {}
}
