namespace EasyCards.Events;

using HarmonyLib;
using RogueGenesia.Data;
using RogueGenesia.GameManager;

[HarmonyPatch]
public static class GameEvents
{
    public delegate void OnRogueLevelStartedHandler();
    public delegate void OnRogueLevelEndedHandler();
    public delegate void OnStartNewGameHandler();
    public delegate void OnGameStartHandler();
    public delegate void OnGameEndHandler();
    public delegate void OnPlayerFinalDeathHandler();

    public static event OnRogueLevelStartedHandler OnRogueLevelStartedEvent;
    public static event OnRogueLevelEndedHandler OnRogueLevelEndedEvent;
    public static event OnStartNewGameHandler OnStartNewGameEvent;
    public static event OnGameStartHandler OnGameStartEvent;
    public static event OnGameEndHandler OnGameEndEvent;
    public static event OnPlayerFinalDeathHandler OnPlayerFinalDeathEvent;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManagerRogue), nameof(GameManagerRogue.Awake))]
    private static void GameManagerRogue_Awake() => OnRogueLevelStartedEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManagerRogue), nameof(GameManagerRogue.LevelEnd))]
    private static void GameManagerRogue_LevelEnd() => OnRogueLevelEndedEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), "OnStartNewGame")]
    private static void GameData_OnStartNewGame() => OnStartNewGameEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), "OnGameStart")]
    private static void GameData_OnGameStart() => OnGameStartEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), "OnGameEnd")]
    private static void GameData_OnGameEnd() => OnGameEndEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), "OnPlayerFinalDeath")]
    private static void GameData_OnPlayerFinalDeathHandler() => OnPlayerFinalDeathEvent?.Invoke();
}
