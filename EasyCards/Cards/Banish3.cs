namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class Banish3 : BanishesOnAcquireCard
{
    protected override int CardValue => 3;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Banish +3" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public Banish3() : base(ClassInjector.DerivedConstructorPointer<Banish3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public Banish3(IntPtr ptr) : base(ptr) {}
}
