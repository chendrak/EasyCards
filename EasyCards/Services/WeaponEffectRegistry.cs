namespace EasyCards.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using UnityEngine;

public static class WeaponEffectRegistry
{
    public delegate void OnWeaponEffectTypeRegistered(Type type);
    public delegate void OnWeaponEffectTypeUnregistered(Type type);

    public static event OnWeaponEffectTypeRegistered OnWeaponEffectRegisteredEvent;
    public static event OnWeaponEffectTypeUnregistered OnWeaponEffectUnregisteredEvent;

    private static Dictionary<System.Type, ConstructorInfo> WeaponEffectTypeToConstructor = new();

    public static void RegisterWeaponEffectType<T>() where T : WeaponEffect
    {
        var type = typeof(T);
        var ctor = type.GetConstructor(Type.EmptyTypes);

        WeaponEffectTypeToConstructor[type] = ctor;
        OnWeaponEffectRegisteredEvent?.Invoke(type);
    }

    public static void UnregisterWeaponEffectType<T>() where T : WeaponEffect
    {
        var type = typeof(T);
        WeaponEffectTypeToConstructor.Remove(type);
        OnWeaponEffectUnregisteredEvent?.Invoke(type);
    }

    public static WeaponEffect? GetEffectForType(Type type)
    {
        if (WeaponEffectTypeToConstructor.TryGetValue(type, out var ctor))
        {
            return ctor.Invoke(Array.Empty<object>()) as WeaponEffect;
        }

        return null;
    }
}
