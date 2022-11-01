namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Banish5 : BanishesOnAcquireCard
{
    protected override int CardValue => 5;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Banish +5" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Banish5() : base(ClassInjector.DerivedConstructorPointer<Banish5>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Banish5(IntPtr ptr) : base(ptr) {}
}
