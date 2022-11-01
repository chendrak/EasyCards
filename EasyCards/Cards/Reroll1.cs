namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Reroll1 : RerollsOnAcquireCard
{
    protected override int NumberOfRerolls => 1;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Reroll +1" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Reroll1() : base(ClassInjector.DerivedConstructorPointer<Reroll1>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Reroll1(IntPtr ptr) : base(ptr) {}
}
