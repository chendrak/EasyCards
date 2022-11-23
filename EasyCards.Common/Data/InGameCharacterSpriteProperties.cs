namespace EasyCards.Common.Data;

using UnityEngine;

public class InGameCharacterSpriteProperties
{
    public InGameSpriteConfiguration? Idle;
    public InGameSpriteConfiguration? Run;
    public InGameSpriteConfiguration? Victory;
    public InGameSpriteConfiguration? Death;
}

public struct InGameSpriteConfiguration
{
    public Texture2D Texture;
    public Vector2 Dimensions;
}
