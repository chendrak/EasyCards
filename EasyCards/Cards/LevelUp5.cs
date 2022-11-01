namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class LevelUp5 : LevelOnAcquireCard
{
    protected override int NumberOfLevelUps => 5;
    protected override CardRarity Rarity => CardRarity.Epic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Level +5" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public LevelUp5() : base(ClassInjector.DerivedConstructorPointer<LevelUp5>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public LevelUp5(IntPtr ptr) : base(ptr) {}
}
