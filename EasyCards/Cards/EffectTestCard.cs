namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Effects;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class EffectTestCard: AbstractEffectCard
{
    protected override float DropWeight => 9999f;
    protected override float LevelUpWeight => 9999f;
    protected override CardRarity Rarity => CardRarity.Ascended;
    protected override CardTag Tags => CardTag.Defence;
    protected override int MaxLevel => 1;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Effect Test Card" }, };
    public override Dictionary<string, string> LocalizedDescriptions => new() { { "en", "Test card for various effects" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public EffectTestCard(IntPtr ptr) : base(ptr) {}
    public EffectTestCard() : this(ClassInjector.DerivedConstructorPointer<EffectTestCard>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    protected override void InitializeEffects()
    {
        this.AddEffect(new LoggingOnKillEffect());
        this.AddEffect(new LoggingOnDashEffect());
        this.AddEffect(new LoggingOnUpdateEffect());
        this.AddEffect(new LoggingMultiEffect());
    }
}
