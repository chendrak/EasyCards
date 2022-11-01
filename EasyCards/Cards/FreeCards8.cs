namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class FreeCards8 : NumberOfCardSelectionsOnAcquireCard
{
    protected override int NumberOfCards => 8;
    protected override CardRarity Rarity => CardRarity.Heroic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "8 Cards" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public FreeCards8() : base(ClassInjector.DerivedConstructorPointer<FreeCards8>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public FreeCards8(IntPtr ptr) : base(ptr) {}
}
