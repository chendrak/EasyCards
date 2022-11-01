namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Reroll5 : RerollsOnAcquireCard
{
    protected override int NumberOfRerolls => 5;
    protected override CardRarity Rarity => CardRarity.Rare;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Reroll +5" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Reroll5() : base(ClassInjector.DerivedConstructorPointer<Reroll5>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Reroll5(IntPtr ptr) : base(ptr) {}
}
