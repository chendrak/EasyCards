using EasyCards.Bootstrap;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModGenesia;

namespace EasyCards
{
    using System.Reflection;
    using Events;
    using HarmonyLib;
    using Version = SemanticVersioning.Version;

    public class EasyCards : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyCards";

        private readonly Version MinimumRequiredGameVersion = new(0, 7, 6, preRelease: ".0");

        public override void OnRegisterModdedContent()
        {
            Log.Debug($"OnRegisterModdedContent");
            CardLoader.Initialize();
        }

        public override void OnAllContentLoaded()
        {
            Log.Debug($"OnAllContentLoaded");
        }

        public override void OnModLoaded(ModData modData)
        {
            Log.Debug($"OnModLoaded({modData})");
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                Log.Error($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.OnModUnloaded();
                return;
            }

            Paths.Initialize(modData.ModDirectory);

            DebugHelper.Initialize();
            GameEvents.OnGameLaunchEvent += EffectHolder.ResetEffects;

            // Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        public override void OnModUnloaded()
        {
            Log.Debug($"OnModUnloaded");
        }
    }
}
