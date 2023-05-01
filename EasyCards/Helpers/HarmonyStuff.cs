namespace EasyCards.Helpers;

using Common.Logging;
using HarmonyLib;
using RogueGenesia.GameManager;

[HarmonyPatch]
public static class HarmonyStuff
{
    [HarmonyPatch(typeof(EnemyManager), nameof(EnemyManager.Awake))]
    [HarmonyPostfix]
    static void JustAPrefix()
    {
        Log.Info("GameManager has awoken");
    }
}
