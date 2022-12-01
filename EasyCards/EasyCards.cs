using BepInEx;
using EasyCards.Bootstrap;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using ModManager;
using EasyCards.CardTypes;
using EasyCards.Common.Helpers;
using EasyCards.Effects;
using EasyCards.Events;

namespace EasyCards
{
    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyCards : RogueGenesiaMod
    {
        internal static EasyCards Instance { get; private set; }
        private static ICardLoader CardLoader { get; set; }

        public override void Load()
        {
            // This must be set, before resolving anything from the container.
            // As the container uses this to expose BepInEx types.
            Instance = this;

            Container.Instance.Resolve<IEasyCardsPluginLoader>().Load();
            CardLoader = Container.Instance.Resolve<ICardLoader>();
            GameEvents.OnGameStartEvent += EffectHolder.ResetEffects;

            HarmonyPatchHelper.ApplyPatches("EasyCards");
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

            var ctor = Il2CppType.TypeFromPointer(ptr).GetConstructor((Il2CppReferenceArray<Il2CppSystem.Type>)System.Array.Empty<Il2CppSystem.Type>());
            var so = ModGenesia.ModGenesia.AddCustomCard(
                card.Name,
                ctor,
                soulCardCreationData
            );
        }
    }
}
