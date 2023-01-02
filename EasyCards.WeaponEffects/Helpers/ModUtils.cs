namespace EasyCards.Common.Helpers;

using Data;
using ModGenesia;
using UnityEngine;

public static class ModUtils
{
    public static void ApplyRogSkin(InGameCharacterSpriteProperties properties)
    {
        ModGenesia.ReplaceRogSkin(
            avatarID: 0,
            idleAnimation: properties.Idle?.Texture,
            idleFrameCount: properties.Idle?.Dimensions ?? Vector2.zero,
            runningAnimation: properties.Run?.Texture,
            runningFrameCount: properties.Run?.Dimensions ?? Vector2.zero,
            victoryAnimation: properties.Victory?.Texture,
            victoryFrameCount: properties.Victory?.Dimensions ?? Vector2.zero,
            deathAnimation: properties.Death?.Texture,
            deathFrameCount: properties.Death?.Dimensions ?? Vector2.zero
        );

    }
}
