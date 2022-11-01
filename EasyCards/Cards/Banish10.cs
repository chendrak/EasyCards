namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Banish10 : BanishesOnAcquireCard
{
    protected override int CardValue => 10;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Banish +10" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Banish10() : base(ClassInjector.DerivedConstructorPointer<Banish10>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Banish10(IntPtr ptr) : base(ptr) {}
}
