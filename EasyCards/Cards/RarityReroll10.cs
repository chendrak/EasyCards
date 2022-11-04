namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class RarityReroll10 : RarityRerollsOnAcquireCard
{
    protected override int CardValue => 10;
    protected override CardRarity Rarity => CardRarity.Rare;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Rarity Rerolls +10" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public RarityReroll10() : base(ClassInjector.DerivedConstructorPointer<RarityReroll10>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public RarityReroll10(IntPtr ptr) : base(ptr) {}
}
