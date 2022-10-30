using BepInEx;
using EasyCards.Bootstrap;
using ModManager;

namespace EasyCards
{
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
        }

        public override string ModDescription()
        {
            var loadedCards = CardLoader.GetLoadedCards();
            return $"Loaded cards: {loadedCards.Count}";
        }
    }
}
