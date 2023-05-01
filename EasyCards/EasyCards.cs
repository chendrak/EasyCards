using EasyCards.Bootstrap;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModGenesia;
using RogueGenesia.GameManager;

namespace EasyCards
{
    using System.Linq;
    using Common.Logging;

    public class EasyCards : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyCards";

        public EasyCards()
        {
            ModOptionHelper.RegisterModOptions();
            var logLevel = ModOptionHelper.AreDebugLogsEnabled() ? Log.LogLevel.DEBUG : Log.LogLevel.INFO;
            Log.Initialize(MOD_NAME);
            Log.SetMinimumLogLevel(logLevel);
        }

        public override void OnRegisterModdedContent()
        {
            Log.Info($"Attempting to find card packs");
            var modPaths = ModLoader.EnabledMods.Select(mod => mod.ModDirectory.FullName).ToList();
            CardLoader.Initialize(modPaths);
        }

        public override void OnModLoaded(ModData modData)
        {
            // This needs to be the first line, because a bunch of stuff relies on the paths being initialized
            Paths.Initialize(modData.ModDirectory);

            Log.Debug($"OnModLoaded({modData})");

            DebugHelper.Initialize();
            GameEventManager.OnGameStart.AddListener(EffectHolder.ResetEffects);
        }

        public override void OnModUnloaded()
        {
            Log.Debug("OnModUnloaded");
            CardLoader.Cleanup();
        }
    }
}
