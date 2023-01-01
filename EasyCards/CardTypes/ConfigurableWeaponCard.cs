namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using Common.Events;
using Effects;
using Il2CppSystem.Reflection;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ConfigurableWeaponCard : Weapon
{
    // private GameObject

    public ConfigurableWeaponCard(IntPtr ptr) : base(ptr) { }
}
