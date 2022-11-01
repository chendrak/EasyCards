namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class LevelUp3 : LevelOnAcquireCard
{
    protected override int NumberOfLevelUps => 3;
    protected override CardRarity Rarity => CardRarity.Uncommon;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Level +3" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public LevelUp3() : base(ClassInjector.DerivedConstructorPointer<LevelUp3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public LevelUp3(IntPtr ptr) : base(ptr) {}
}
