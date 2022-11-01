namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class RarityReroll3 : BanishesOnAcquireCard
{
    protected override int CardValue => 3;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Rarity Reroll +3" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public RarityReroll3() : base(ClassInjector.DerivedConstructorPointer<RarityReroll3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public RarityReroll3(IntPtr ptr) : base(ptr) {}
}
