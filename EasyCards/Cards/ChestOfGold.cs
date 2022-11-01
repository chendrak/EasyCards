namespace EasyCards;

using System;
using System.Collections.Generic;
using BepInEx.Logging;
using CardTypes;
using Extensions;
using Helpers;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using ModGenesia;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ChestOfGold : GoldOnAcquireCard
{
    protected override int GoldAmountToAdd => 10_000;
    protected override CardRarity Rarity => CardRarity.Epic;
    protected override Dictionary<string, string> LocalizedNames => new() { { "en", "Chest of Gold" }, };
    protected override string TexturePath => Path.Combine(Paths.Assets, "chest_of_gold.png");

    public ChestOfGold() : base(ClassInjector.DerivedConstructorPointer<ChestOfGold>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public ChestOfGold(IntPtr ptr) : base(ptr) {}
}
