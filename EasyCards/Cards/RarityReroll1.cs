namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class RarityReroll1 : RarityRerollsOnAcquireCard
{
    protected override int CardValue => 1;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Rarity Reroll +1" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public RarityReroll1() : base(ClassInjector.DerivedConstructorPointer<RarityReroll1>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public RarityReroll1(IntPtr ptr) : base(ptr) {}
}
