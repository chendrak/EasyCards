namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class LooseChange : GoldOnAcquireCard
{
    protected override int GoldAmountToAdd => 100;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Loose Change" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "loose_change.png");

    public LooseChange() : base(ClassInjector.DerivedConstructorPointer<LooseChange>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public LooseChange(IntPtr ptr) : base(ptr) {}
}
