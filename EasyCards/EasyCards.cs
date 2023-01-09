using EasyCards.Bootstrap;
using EasyCards.Common.Events;
using EasyCards.Common.Helpers;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModManager;

namespace EasyCards
{
    using System;
    using System.IO;
    using BepInEx;
    using Mono.Cecil;
    using Services;
    using UnityEngine.InputSystem;
    using Version = SemanticVersioning.Version;

    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyCards : RogueGenesiaMod
    {
        internal static EasyCards Instance { get; private set; }

        private readonly Version MinimumRequiredGameVersion = new(0, 7, 2, preRelease: ".0b-beta");

        public override void Load()
        {
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                Log.LogError(
                    $"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.Unload();
                return;
            }

            // This must be set, before resolving anything from the container.
            // As the container uses this to expose BepInEx types.
            Instance = this;

            DebugHelper.Initialize();
            ResourceHelper.Initialize();

            GameEvents.OnGameLaunchEvent += EffectHolder.ResetEffects;

            WeaponEffectLoader.Initialize(CreateAssemblyResolver());

            WeaponToEffectMapper.RegisterWeaponToEffectMapping("TestWeapon", "TestEffect");

            KeyPressHelper.RegisterKey(Key.F6, Modifiers.None, ReloadWeaponEffects);

            HarmonyPatchHelper.ApplyPatches("EasyCards");

            // This should be the last thing we initialize, so the cards get loaded at the very end
            CardLoader.Initialize();
        }

        private void ReloadWeaponEffects()
        {
            var weaponEffectDirectory = Path.Combine(Paths.EasyCards, "weapon-effects");
            WeaponEffectLoader.LoadWeaponEffectsFromPath(weaponEffectDirectory);
        }

        private static DefaultAssemblyResolver CreateAssemblyResolver()
        {
            var weaponEffectDirectory = Path.Combine(Paths.EasyCards, "weapon-effects");
            var interopDirectory = Path.Combine(BepInEx.Paths.BepInExRootPath, "interop");

            var defaultResolver = new DefaultAssemblyResolver();
            defaultResolver.AddSearchDirectory(weaponEffectDirectory);
            defaultResolver.AddSearchDirectory(interopDirectory);
            defaultResolver.AddSearchDirectory(BepInEx.Paths.ManagedPath);
            defaultResolver.AddSearchDirectory(BepInEx.Paths.BepInExAssemblyDirectory);

            return defaultResolver;
        }

        public override string ModDescription()
        {
            var loadedCards = CardLoader.GetLoadedCards();
            return $"Loaded cards: {loadedCards.Count}";
        }
    }
}
