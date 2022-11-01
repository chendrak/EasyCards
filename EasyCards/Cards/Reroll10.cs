namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Reroll10 : RerollsOnAcquireCard
{
    protected override int NumberOfRerolls => 10;
    protected override CardRarity Rarity => CardRarity.Epic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Reroll +10" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Reroll10() : base(ClassInjector.DerivedConstructorPointer<Reroll10>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Reroll10(IntPtr ptr) : base(ptr) {}
}
