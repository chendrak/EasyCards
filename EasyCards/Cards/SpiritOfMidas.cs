namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public class SpiritOfMidas : TimeLimitedEffectCard
{
    public SpiritOfMidas(IntPtr ptr) : base(ptr) {}
    public SpiritOfMidas() : base(ClassInjector.DerivedConstructorPointer<SpiritOfMidas>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    protected override CardRarity Rarity => CardRarity.Heroic;
    protected override CardTag Tags => CardTag.None;
    protected override int MaxLevel => 1;
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;

    protected override float TotalActiveTimeInSeconds => 30.0f;
    protected override Dictionary<string, string> LocalizedNames => new() { ["en"] = $"Spirit of Midas", };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public override Dictionary<string, string> LocalizedDescriptions => new()
    {
        ["en"] = $"Grants 1 gold on kill for {(int)this.TotalActiveTimeInSeconds} seconds after the start of a stage",
    };

    public override void OnKill(PlayerData owner, IEntity killedEntity)
    {
        var go = killedEntity.GetLinkedGameObject();

        if (this.IsActive)
        {
            GameData.PlayerDatabase[0].gold += 1;
        }
    }
}
