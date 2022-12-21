namespace EasyCards.Bootstrap;

using Common.Helpers;
using Events;

public static class ResourceHelper
{
    public static void Initialize()
    {
        GameResources.Initialize();
        GameEvents.OnRunEndEvent += OnRunEnded;
    }

    private static void OnRunEnded()
    {
        // Restore original sprite
        ModUtils.ApplyRogSkin(GameResources.DefaultCharacterSpriteProperties);
    }
}
