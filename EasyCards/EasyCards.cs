using BepInEx;
using EasyCards.Bootstrap;
using EasyCards.CardTypes;
using EasyCards.Common.Events;
using EasyCards.Common.Helpers;
using EasyCards.Effects;
using EasyCards.Helpers;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using ModManager;
using SemanticVersioning;
using ModGenesia;

namespace EasyCards
{
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
                Log.LogError($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.Unload();
                return;
            }

            // This must be set, before resolving anything from the container.
            // As the container uses this to expose BepInEx types.
            Instance = this;

            DebugHelper.Initialize();
            GameEvents.OnGameLaunchEvent += EffectHolder.ResetEffects;

            HarmonyPatchHelper.ApplyPatches("EasyCards");

            // This should be the last thing we initialize, so the cards get loaded at the very end
            CardLoader.Initialize();
        }

        public override string ModDescription()
        {
            var loadedCards = CardLoader.GetLoadedCards();
            return $"Loaded cards: {loadedCards.Count}";
        }

        private static void AddCustomCard<T>() where T : CustomSoulCard
        {
            var classType = typeof(T);
            ClassInjector.RegisterTypeInIl2Cpp(classType);
            var card = System.Activator.CreateInstance<T>();
            var soulCardCreationData = card.GetSoulCardCreationData();
            var ptr = IL2CPP.GetIl2CppClass(
                "Assembly-CSharp.dll",
                classType.Namespace,
                classType.Name
            );

            var ctor = Il2CppType.TypeFromPointer(ptr)
                .GetConstructor((Il2CppReferenceArray<Il2CppSystem.Type>)System.Array.Empty<Il2CppSystem.Type>());
            var so = CardAPI.AddCustomCard(
                card.Name,
                ctor,
                soulCardCreationData
            );
        }
    }
}
