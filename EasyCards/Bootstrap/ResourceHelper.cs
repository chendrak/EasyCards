namespace EasyCards.Bootstrap;

using Common.Helpers;
using Events;

public class ResourceHelper : IResourceHelper
{
    public void Initialize()
    {
        GameResources.Initialize();
        GameEvents.OnRunEndEvent += this.OnRunEnded;
    }

    private void OnRunEnded()
    {
        // Restore original sprite
        ModUtils.ApplyRogSkin(GameResources.DefaultCharacterSpriteProperties);
    }
}