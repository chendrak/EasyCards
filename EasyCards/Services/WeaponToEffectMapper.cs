namespace EasyCards.Services;

using System.Collections.Generic;
using Models.Templates;

public static class WeaponToEffectMapper
{
    private static Dictionary<string, string> WeaponToEffectMap = new();

    public static void RegisterWeaponToEffectMapping(string weaponTypeName, string effectName)
    {
        WeaponToEffectMap[weaponTypeName] = effectName;
    }

    public static string? GetEffectNameForWeapon(string weaponName)
    {
        return WeaponToEffectMap.TryGetValue(weaponName, out var effectName) ? effectName : null;
    }

    public static WeaponEffect? GetEffectForWeapon(string weaponName)
    {
        return WeaponToEffectMap.TryGetValue(weaponName, out var effectName)
            ? WeaponEffectRegistry.GetEffectForName(effectName)
            : null;
    }
}
