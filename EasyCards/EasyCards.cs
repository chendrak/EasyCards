using BepInEx;
using EasyCards.Bootstrap;
using ModManager;

using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace EasyCards
{
    using System;
    using System.Linq;
    using CardTypes;
    using Effects;
    using Events;
    using Extensions;
    using Helpers;

    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyCards : RogueGenesiaMod
    {
        internal static EasyCards Instance { get; private set; }
        private static ICardLoader CardLoader { get; set; }

        private System.Type[] cachedTypes = System.Array.Empty<Type>();

        public override void Load()
        {
            // This must be set, before resolving anything from the container.
            // As the container uses this to expose BepInEx types.
            Instance = this;
            Container.Instance.Resolve<IEasyCardsPluginLoader>().Load();
            CardLoader = Container.Instance.Resolve<ICardLoader>();

            this.AddCustomCards();

            GameEvents.OnGameStartEvent += EffectHolder.ResetEffects;
        }

        public override string ModDescription()
        {
            var loadedCards = CardLoader.GetLoadedCards();
            return $"Loaded cards: {loadedCards.Count}";
        }

        public void AddCustomCards()
        {
            // this.LogCardsAndEffects();
            // this.RegisterEffects();

            // AddCustomCard<LooseChange>();
            // AddCustomCard<GoldenPearl>();
            // AddCustomCard<ChestOfGold>();
            // AddCustomCard<BarrelOfGold>();

            // AddCustomCard<Banish1>();
            // AddCustomCard<Banish3>();
            // AddCustomCard<Banish5>();
            // AddCustomCard<Banish10>();
            // AddCustomCard<RarityReroll1>();
            // AddCustomCard<RarityReroll3>();
            // AddCustomCard<RarityReroll5>();
            // AddCustomCard<RarityReroll10>();
            // AddCustomCard<Reroll1>();
            // AddCustomCard<Reroll3>();
            // AddCustomCard<Reroll5>();
            // AddCustomCard<Reroll10>();
            // AddCustomCard<SpiritOfMidas>();
            // AddCustomCard<EffectTestCard>();
        }

        private void WarmupTypeCacheIfNecessary()
        {
            if (this.cachedTypes.Length == 0)
            {
                this.cachedTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .ToArray();
            }
        }

        private void RegisterEffects()
        {
            Instance.Log.LogInfo($"Registering effects");
            this.WarmupTypeCacheIfNecessary();
            var effectTypes = this.cachedTypes
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AbstractEffect)))
                .ToArray();

            Instance.Log.LogInfo($"Got {effectTypes.Length} effects");
            foreach (var effectType in effectTypes)
            {
                Instance.Log.LogInfo($"Registering {effectType.Name}");
                ClassInjector.RegisterTypeInIl2Cpp(effectType);
            }
        }

        private void LogCardsAndEffects()
        {
            this.WarmupTypeCacheIfNecessary();
            var effectTypes = this.cachedTypes
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AbstractEffect)))
                .ToList();

            Instance.Log.LogInfo($"Got {effectTypes.Count} effects");
            foreach (var effectType in effectTypes)
            {
                Instance.Log.LogInfo($"{effectType.FullName}");
            }

            var cardTypes = this.cachedTypes
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(CustomSoulCard))).ToList();

            Instance.Log.LogInfo($"Got {cardTypes.Count} cards");
            foreach (var cardType in cardTypes)
            {
                Instance.Log.LogInfo($"{cardType.FullName}");
            }
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

            var ctor = Il2CppType.TypeFromPointer(ptr).GetConstructor((Il2CppReferenceArray<Il2CppSystem.Type>)System.Array.Empty<Il2CppSystem.Type>());
            var so = ModGenesia.ModGenesia.AddCustomCard(
                card.Name,
                ctor,
                soulCardCreationData
            );
        }
    }
}
