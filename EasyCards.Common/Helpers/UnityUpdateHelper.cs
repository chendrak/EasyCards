namespace EasyCards.Common.Helpers;

using HarmonyLib;

[HarmonyPatch]
public static class UnityUpdateHelper
{
    public delegate void UnityUpdateHandler();
    public static event UnityUpdateHandler OnUnityUpdate;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SteamManager), nameof(SteamManager.Update))]
    private static void SteamManager_Update()
    {
        OnUnityUpdate?.Invoke();
    }

    public static void Cleanup()
    {
        OnUnityUpdate = null;
    }
}
