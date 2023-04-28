namespace EasyCards.Helpers;

using System.Collections.Generic;
using ModGenesia;
using RogueGenesia.Data;

public static class ModOptionHelper
{
    private static string DEBUG_LOG_KEY = $"{EasyCards.MOD_NAME}_DEBUG_LOG";

    public static void RegisterModOptions()
    {
        RegisterDebugLogOption();
    }

    private static void RegisterDebugLogOption()
    {
        var textLocalization = new LocalizationDataList
        {
            localization = new List<LocalizationData> { new() { Key = "en", Value = "Enable debug logs" } }
        };
        var tooltipLocalization = new LocalizationDataList
        {
            localization = new List<LocalizationData> { new() { Key = "en", Value = "Enable debug logging for EasyCards. This is mostly useful for card pack creators or when you are trying to debug issues. This requires a restart of the game!" } }
        };

        var debugLogOption =
            ModOption.MakeToggleOption(name: DEBUG_LOG_KEY, LocalisedName: textLocalization, defaultValue: false, LocalisedTooltip: tooltipLocalization);

        ModOption.AddModOption(optionData: debugLogOption, Category: "Debug", Tabs: EasyCards.MOD_NAME);
    }

    public static bool AreDebugLogsEnabled() => (int)GameData.OptionData.GetValue(DEBUG_LOG_KEY) == 1;
}
