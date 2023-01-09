namespace EasyCards.Services;

using System;
using System.Collections.Generic;
using System.Reflection;
using Models.Templates;

public static class WeaponEffectRegistry
{
    public delegate void OnWeaponEffectTypeRegistered(Type type);

    public delegate void OnWeaponEffectTypeUnregistered(Type type);

    public static event OnWeaponEffectTypeRegistered OnWeaponEffectRegisteredEvent;
    public static event OnWeaponEffectTypeUnregistered OnWeaponEffectUnregisteredEvent;

    private static Dictionary<Type, ConstructorInfo> WeaponEffectTypeToConstructor = new();

    public static void RegisterWeaponEffectType(Type type)
    {
        if (IsWeaponEffectType(type))
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);

            WeaponEffectTypeToConstructor[type] = ctor;
            OnWeaponEffectRegisteredEvent?.Invoke(type);
        }
    }

    public static void UnregisterWeaponEffectType(Type type)
    {
        if (IsWeaponEffectType(type))
        {
            WeaponEffectTypeToConstructor.Remove(type);
            OnWeaponEffectUnregisteredEvent?.Invoke(type);
        }
    }

    private static bool IsWeaponEffectType(Type type) => typeof(WeaponEffect).IsAssignableFrom(type);

    public static WeaponEffect? GetEffectForType(Type type)
    {
        if (WeaponEffectTypeToConstructor.TryGetValue(type, out var ctor))
        {
            return ctor.Invoke(Array.Empty<object>()) as WeaponEffect;
        }

        return null;
    }

    public static WeaponEffect? GetEffectForName(string weaponEffectName)
    {
        foreach (var (type, ctor) in WeaponEffectTypeToConstructor)
        {
            if (type.Name == weaponEffectName)
            {
                return ctor.Invoke(Array.Empty<object>()) as WeaponEffect;
            }
        }

        return null;
    }
}
