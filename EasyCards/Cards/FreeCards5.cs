namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class FreeCards5 : NumberOfCardSelectionsOnAcquireCard
{
    protected override int NumberOfCards => 5;
    protected override CardRarity Rarity => CardRarity.Epic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "5 Cards" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public FreeCards5() : base(ClassInjector.DerivedConstructorPointer<FreeCards5>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public FreeCards5(IntPtr ptr) : base(ptr) {}
}
