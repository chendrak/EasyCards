namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Banish1 : BanishesOnAcquireCard
{
    protected override int CardValue => 1;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Banish +1" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Banish1() : base(ClassInjector.DerivedConstructorPointer<Banish1>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Banish1(IntPtr ptr) : base(ptr) {}
}
