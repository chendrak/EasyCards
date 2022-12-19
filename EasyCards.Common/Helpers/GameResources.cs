namespace EasyCards.Common.Helpers;

using Data;
using Events;
using RogueGenesia.Data;
using UnityEngine;

public static class GameResources
{
    public static InGameCharacterSpriteProperties DefaultCharacterSpriteProperties;

    public static void Initialize()
    {
        GameEvents.OnGameLaunchEvent += OnGameStarted;
    }

    private static void OnGameStarted()
    {
        DefaultCharacterSpriteProperties = new()
        {
            Idle = new InGameSpriteConfiguration
            {
                Texture = GameData.PlayerIdleAnimation[0],
                Dimensions = GameData.PlayerIdleFrameCount[0],
            },
            Run = new InGameSpriteConfiguration
            {
                Texture = GameData.PlayerRunAnimation[0],
                Dimensions = GameData.PlayerRunFrameCount[0],
            },
            Victory = new InGameSpriteConfiguration
            {
                Texture = GameData.PlayerVictoryAnimation[0],
                Dimensions = GameData.PlayerVictoryFrameCount[0],
            },
            Death = new InGameSpriteConfiguration
            {
                Texture = GameData.PlayerDeathAnimation[0],
                Dimensions = GameData.PlayerDeathFrameCount[0],
            },
        };
    }
}
