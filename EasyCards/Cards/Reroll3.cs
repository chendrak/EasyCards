namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Reroll3 : RerollsOnAcquireCard
{
    protected override int NumberOfRerolls => 3;
    protected override CardRarity Rarity => CardRarity.Uncommon;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Reroll +3" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Reroll3() : base(ClassInjector.DerivedConstructorPointer<Reroll3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Reroll3(IntPtr ptr) : base(ptr) {}
}
