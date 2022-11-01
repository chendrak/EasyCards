namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class FreeCards3 : NumberOfCardSelectionsOnAcquireCard
{
    protected override int NumberOfCards => 3;
    protected override CardRarity Rarity => CardRarity.Rare;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "3 Cards" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public FreeCards3() : base(ClassInjector.DerivedConstructorPointer<FreeCards3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public FreeCards3(IntPtr ptr) : base(ptr) {}
}
