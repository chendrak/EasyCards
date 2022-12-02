namespace EasyCards.Events;

using HarmonyLib;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using RogueGenesia.UI;
using UnityEngine;

[HarmonyPatch]
public static class GameEvents
{
    public delegate void OnRogueLevelStartedHandler();
    public delegate void OnRogueLevelEndedHandler();
    public delegate void OnStartNewGameHandler();
    public delegate void OnGameStartHandler();
    public delegate void OnRunEndHandler();
    public delegate void OnPlayerFinalDeathHandler();
    public delegate void OnDeathHandler();
    public delegate void OnPlayerTakeDamageHandler();
    public delegate void OnSoulCardTakeDamageHandler();

    public static event OnRogueLevelStartedHandler OnRogueLevelStartedEvent;
    public static event OnRogueLevelEndedHandler OnRogueLevelEndedEvent;
    public static event OnStartNewGameHandler OnStartNewGameEvent;
    public static event OnGameStartHandler OnGameStartEvent;
    public static event OnRunEndHandler OnRunEndEvent;
    public static event OnPlayerFinalDeathHandler OnPlayerFinalDeathEvent;
    public static event OnDeathHandler OnDeathEvent;
    public static event OnPlayerTakeDamageHandler OnPlayerTakeDamageEvent;
    public static event OnSoulCardTakeDamageHandler OnSoulCardTakeDamageEvent;

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
    [HarmonyPatch(typeof(GameEndManager), nameof(GameEndManager.Awake))]
    [HarmonyPatch(typeof(PauseMenu), nameof(PauseMenu.On_Confirm))]
    private static void OnRunEnded() => OnRunEndEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnPlayerFinalDeath))]
    private static void GameData_OnPlayerFinalDeathHandler() => OnPlayerFinalDeathEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.OnDeath))]
    private static void PlayerEntity_OnDeathHandler() => OnDeathEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerEntity), nameof(PlayerEntity.TakeDamage))]
    private static void PlayerEntity_OnTakeDamageHandler(DamageInformation damageInfo)
    {
        Debug.Log($"PlayerEntity_OnTakeDamageHandler({damageInfo})");
        OnPlayerTakeDamageEvent?.Invoke();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Monster), nameof(Monster.DealDamageToPlayer))]
    private static void Monster_DealDamageToPlayerHandler()
    {
        Debug.Log("Monster_DealDamageToPlayerHandler");
        OnPlayerTakeDamageEvent?.Invoke();
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(Lust), nameof(Lust.OnTakeDamage))]
    // private static void Lust_OnTakeDamageHandler()
    // {
    //     Debug.Log($"Lust_OnTakeDamageHandler()");
    // }
    //
    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(Benediction), nameof(Benediction.OnTakeDamage))]
    // private static void Benediction_OnTakeDamageHandler()
    // {
    //     Debug.Log($"Benediction_OnTakeDamageHandler()");
    // }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(SoulCard), nameof(SoulCard.OnTakeDamage))]
    // private static void SoulCard_OnTakeDamageHandler()
    // {
    //     Debug.Log("SoulCard_OnTakeDamageHandler()");
    //     // OnSoulCardTakeDamageEvent?.Invoke(owner, damageOwner, modifierDamageValue, damageValue);
    // }
}
