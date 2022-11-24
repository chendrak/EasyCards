namespace EasyCards.Common.Helpers;

using System.Reflection;
using HarmonyLib;

// This will only patch anything that is defined in [EasyCards.Common]. The system should be updated
// to search for a base Interface and patch based on that.
public static class HarmonyPatchHelper
{
    public static void ApplyPatches()
    {
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }
}
