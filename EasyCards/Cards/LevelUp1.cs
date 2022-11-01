namespace EasyCards;

using System;
using System.Collections.Generic;
using CardTypes;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using RogueGenesia.Data;

public class LevelUp1 : LevelOnAcquireCard
{
    protected override int NumberOfLevelUps => 1;
    protected override CardRarity Rarity => CardRarity.Normal;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Level +1" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "placeholder.png");

    public LevelUp1() : base(ClassInjector.DerivedConstructorPointer<LevelUp1>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public LevelUp1(IntPtr ptr) : base(ptr) {}
}
