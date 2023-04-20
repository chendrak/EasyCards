using EasyCards.Bootstrap;
using EasyCards.Effects;
using EasyCards.Helpers;
using ModGenesia;
using RogueGenesia.GameManager;

namespace EasyCards
{

    public class EasyCards : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyCards";

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
            // This needs to be the first line, because a bunch of stuff relies on the paths being initialized
            Paths.Initialize(modData.ModDirectory);

            Log.Debug($"OnModLoaded({modData})");

            DebugHelper.Initialize();
            GameEventManager.OnGameStart.AddListener(EffectHolder.ResetEffects);
        }

        public override void OnModUnloaded()
        {
            Log.Debug($"OnModUnloaded");
        }
    }
}
