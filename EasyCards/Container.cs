using BepInEx.Configuration;
using BepInEx.Logging;
using EasyCards.Bootstrap;
using EasyCards.Logging;
using EasyCards.Services;
using EasyCards.Validation;
using StrongInject;
using StrongInject.Modules;

namespace EasyCards;

[RegisterModule(typeof(LoggingModule))]
[RegisterModule(typeof(BootstrapModule))]
[RegisterModule(typeof(ServicesModule))]
[RegisterModule(typeof(CollectionsModule))]
[RegisterModule(typeof(ValidationModule))]
public sealed partial class Container : IContainer<IEasyCardsPluginLoader>, IContainer<IInputEventSubscriber[]>, IContainer<ICardLoader>
{
    public static Container Instance { get; } = new();
    public T Resolve<T>()
    {
        return ((IContainer<T>)(object)this).Resolve().Value;
    }

    [Factory]
    private EasyCards CreateEasyCards() => EasyCards.Instance;

    [Factory]
    private ManualLogSource CreateManualLogSource(EasyCards easyCards) => easyCards.Log;

    [Factory]
    private ConfigFile CreateConfigFile(EasyCards easyCards) => easyCards.Config;
}
