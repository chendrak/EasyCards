namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class BarrelOfGold : GoldOnAcquireCard
{
    protected override int GoldAmountToAdd => 100_000;
    protected override CardRarity Rarity => CardRarity.Heroic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Barrel of Gold" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "barrel_of_gold.png");

    public BarrelOfGold() : base(ClassInjector.DerivedConstructorPointer<BarrelOfGold>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public BarrelOfGold(IntPtr ptr) : base(ptr) {}
}
