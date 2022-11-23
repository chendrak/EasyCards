namespace EasyCards.Events;

using HarmonyLib;
using RogueGenesia.Actors.Survival;
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
    public delegate void OnDeathHandler();

    public static event OnRogueLevelStartedHandler OnRogueLevelStartedEvent;
    public static event OnRogueLevelEndedHandler OnRogueLevelEndedEvent;
    public static event OnStartNewGameHandler OnStartNewGameEvent;
    public static event OnGameStartHandler OnGameStartEvent;
    public static event OnGameEndHandler OnGameEndEvent;
    public static event OnPlayerFinalDeathHandler OnPlayerFinalDeathEvent;
    public static event OnDeathHandler OnDeathEvent;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManagerRogue), nameof(GameManagerRogue.Awake))]
    private static void GameManagerRogue_Awake() => OnRogueLevelStartedEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManagerRogue), nameof(GameManagerRogue.NextLevel))]
    private static void GameManagerRogue_LevelEnd() => OnRogueLevelEndedEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnStartNewGame))]
    private static void GameData_OnStartNewGame() => OnStartNewGameEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnGameStart))]
    private static void GameData_OnGameStart() => OnGameStartEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnGameEnd))]
    private static void GameData_OnGameEnd() => OnGameEndEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnPlayerFinalDeath))]
    private static void GameData_OnPlayerFinalDeathHandler() => OnPlayerFinalDeathEvent?.Invoke();

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.OnDeath))]
    private static void PlayerEntity_OnDeathHandler() => OnDeathEvent?.Invoke();
}
