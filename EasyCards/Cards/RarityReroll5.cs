namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class RarityReroll5 : RarityRerollsOnAcquireCard
{
    protected override int CardValue => 5;
    protected override CardRarity Rarity => CardRarity.Rare;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Rarity Reroll +5" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public RarityReroll5() : base(ClassInjector.DerivedConstructorPointer<RarityReroll5>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public RarityReroll5(IntPtr ptr) : base(ptr) {}
}
