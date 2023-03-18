using EasyCards.Bootstrap;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModGenesia;

namespace EasyCards
{
    using System.Reflection;
    using Events;
    using HarmonyLib;
    using SemanticVersioning;

    public class EasyCards : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyCards";
        internal static EasyCards Instance { get; private set; }

        private readonly Version MinimumRequiredGameVersion = new(0, 7, 6, preRelease: ".0");

        public override void OnGameFinishedLoading()
        {
            Log.Debug($"OnGameFinishedLoading");
        }

        public override void GameRegisterationStep()
        {
            Log.Debug($"GameRegisterationStep");
            CardLoader.Initialize();
        }

        public override void OnModLoaded(ModData modData)
        {
            Log.Debug($"OnModLoaded({modData})");
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                Log.Error($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.OnModUnloaded(modData);
                return;
            }

            // This must be set, before resolving anything from the container.
            // As the container uses this to expose BepInEx types.
            Instance = this;

            DebugHelper.Initialize();
            GameEvents.OnGameLaunchEvent += EffectHolder.ResetEffects;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnModUnloaded(ModData modData)
        {

        }
    }
}
