namespace EasyCards.Common.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

public static class HarmonyPatchHelper
{
    private static Type[] cachedTypes = Array.Empty<Type>();

    private static List<Harmony> harmonyInstances = new();

    public static void ForceRefreshCachedTypes()
    {
        cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .ToArray();
    }

    private static void WarmupTypeCacheIfNecessary()
    {
        if (cachedTypes.Length == 0)
        {
            ForceRefreshCachedTypes();
        }
    }

    public static void ApplyPatches(params string[] namespaces)
    {
        WarmupTypeCacheIfNecessary();
        if (namespaces.Length == 0)
        {
            Debug.Log($"No namespaces provided, nothing to patch...");
            return;
        }

        var classesWithHarmonyPatches = cachedTypes.Where(type =>
            type.IsClass && type.GetCustomAttribute(typeof(HarmonyPatch), true) != null).ToList();

        Debug.Log($"Found {classesWithHarmonyPatches.Count} classes with [HarmonyPatch] attributes");
        foreach (var classWithHarmonyPatch in classesWithHarmonyPatches)
        {
            var classNamespace = classWithHarmonyPatch.Namespace;

            if (classNamespace != null)
            {
                if (namespaces.Any(namespaceRegex => Regex.IsMatch(classNamespace, namespaceRegex)))
                {
                    Debug.Log($"Found a namespace match in {classWithHarmonyPatch.FullName}, applying patches...");
                    var harmonyForCurrentClass = Harmony.CreateAndPatchAll(classWithHarmonyPatch);
                    harmonyInstances.Add(harmonyForCurrentClass);
                }
                else
                {
                    Debug.Log($"No matches for {classNamespace}");
                }
            }
        }
    }
}
