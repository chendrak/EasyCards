namespace EasyCards.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Logging;
using Models.Templates;
using Mono.Cecil;

public static class WeaponEffectLoader
{
    private static IAssemblyResolver assemblyResolver;

    private static Dictionary<string, List<Type>> knownWeaponEffectTypes = new();

    public static void Initialize(BaseAssemblyResolver assemblyResolver)
    {
        WeaponEffectLoader.assemblyResolver = assemblyResolver;
    }

    public static void LoadWeaponEffectsFromPath(string path)
    {
        var files = Directory.GetFiles(path, "*.dll");
        foreach (var file in files)
        {
            LoadWeaponEffectsFromDLL(file);
        }
    }

    public static void LoadWeaponEffectsFromDLL(string path)
    {
        if (assemblyResolver == null)
        {
            Log.Error("AssemblyResolver wasn't set in WeaponEffectLoader! Call Initialize first before you call LoadDLL!");
            return;
        }

        Log.Info($"Loading WeaponEffects from {path}");

        using var dll =
            AssemblyDefinition.ReadAssembly(path, new ReaderParameters { AssemblyResolver = assemblyResolver });
        dll.Name.Name = $"{dll.Name.Name}-{DateTime.Now.Ticks}";

        using var ms = new MemoryStream();

        dll.Write(ms);
        var ass = Assembly.Load(ms.ToArray());

        var fullAssemblyName = ass.GetName().FullName;
        UnregisterTypesForAssembly(fullAssemblyName);

        var weaponEffectType = typeof(WeaponEffect);
        var weaponEffectTypes = GetTypesSafe(ass).Where(type => weaponEffectType.IsAssignableFrom(type));
        foreach (Type type in weaponEffectTypes)
        {
            WeaponEffectRegistry.RegisterWeaponEffectType(type);
        }
    }

    private static void UnregisterTypesForAssembly(string fullAssemblyName)
    {
        if (knownWeaponEffectTypes.TryGetValue(fullAssemblyName, out var knownTypes))
        {
            foreach (var knownType in knownTypes)
            {
                WeaponEffectRegistry.UnregisterWeaponEffectType(knownType);
            }
        }
    }

    private static IEnumerable<Type> GetTypesSafe(Assembly ass)
    {
        try
        {
            return ass.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            var sbMessage = new StringBuilder();
            sbMessage.AppendLine("\r\n-- LoaderExceptions --");
            foreach (var l in ex.LoaderExceptions)
                sbMessage.AppendLine(l.ToString());
            sbMessage.AppendLine("\r\n-- StackTrace --");
            sbMessage.AppendLine(ex.StackTrace);
            Log.Error(sbMessage.ToString());
            return ex.Types.Where(x => x != null);
        }
    }

    private static IEnumerator DelayAction(Action action)
    {
        yield return null;
        action();
    }
}
