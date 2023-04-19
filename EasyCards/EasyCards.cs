using EasyCards.Bootstrap;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModGenesia;
using RogueGenesia.GameManager;
using UnityEngine;
using Version = SemanticVersioning.Version;

namespace EasyCards
{

    public class EasyCards : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyCards";

        private readonly Version MinimumRequiredGameVersion = new(0, 8, 2, preRelease: ".0");

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
            Debug.Log($"OnModLoaded({modData})");

            // This needs to be the first line, because a bunch of stuff relies on the paths being initialized
            Paths.Initialize(modData.ModDirectory);


            Log.Debug($"OnModLoaded({modData})");
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                Log.Error($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.OnModUnloaded();
                return;
            }

            DebugHelper.Initialize();
            GameEventManager.OnGameStart.AddListener(EffectHolder.ResetEffects);
        }

        public override void OnModUnloaded()
        {
            Log.Debug($"OnModUnloaded");
        }
    }
}
