namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class GoldenPearl : GoldOnAcquireCard
{
    protected override int GoldAmountToAdd => 1_000;
    protected override CardRarity Rarity => CardRarity.Rare;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Golden Pearl" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "golden_pearl.png");

    public GoldenPearl() : base(ClassInjector.DerivedConstructorPointer<GoldenPearl>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public GoldenPearl(IntPtr ptr) : base(ptr) {}
}
