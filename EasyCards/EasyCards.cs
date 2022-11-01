using BepInEx;
using EasyCards.Bootstrap;
using ModManager;

using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Type = Il2CppSystem.Type;

namespace EasyCards
{
    using CardTypes;
    using Extensions;
    using Helpers;

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

            // ModGenesia.ModGenesia.AddCustomCard("GoldenGoose", type.)

            AddCustomCards();
        }

        public override string ModDescription()
        {
            var loadedCards = CardLoader.GetLoadedCards();
            return $"Loaded cards: {loadedCards.Count}";
        }

        public static void AddCustomCards()
        {
            // AddCustomCard<LooseChange>();
            // AddCustomCard<GoldenPearl>();
            // AddCustomCard<ChestOfGold>();
            // AddCustomCard<BarrelOfGold>();

            AddCustomCard<Banish1>();
            AddCustomCard<Banish3>();
            AddCustomCard<Banish5>();
            AddCustomCard<Banish10>();
            AddCustomCard<RarityReroll1>();
            AddCustomCard<RarityReroll3>();
            AddCustomCard<RarityReroll5>();
            AddCustomCard<RarityReroll10>();
            AddCustomCard<Reroll1>();
            AddCustomCard<Reroll3>();
            AddCustomCard<Reroll5>();
            AddCustomCard<Reroll10>();

            // These don't work right now
            // AddCustomCard<FreeCards3>();
            // AddCustomCard<FreeCards5>();
            // AddCustomCard<FreeCards8>();
            // AddCustomCard<LevelUp1>();
            // AddCustomCard<LevelUp3>();
            // AddCustomCard<LevelUp5>();
        }

        private static void AddCustomCard<T>() where T: CustomSoulCard
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

            var ctor = Il2CppType.TypeFromPointer(ptr).GetConstructor((Il2CppReferenceArray<Type>)System.Array.Empty<Type>());
            var so = ModGenesia.ModGenesia.AddCustomCard(
                card.Name,
                ctor,
                soulCardCreationData
            );

            so.DescriptionOverride = Localization.GetTranslations(card.LocalizedDescriptions).ToIl2CppList();
        }
    }
}
