using System;
using System.IO;
using UnityEngine;
using UniverseLib;
using UniverseLib.UI;

namespace EasyCards.UI;

internal class UiManager
{
    public static UIBase UiBase { get; private set; }
    public static CardPanel CardPanel { get; private set; }
    
    internal static void Initialize()
    {
        const float startupDelay = 3f;
        // Universe.Init(OnInitialized, LogHandler);
        UniverseLib.Config.UniverseLibConfig config = new()
        {
            Disable_EventSystem_Override = false, // or null
            Force_Unlock_Mouse = false, // or null
            Allow_UI_Selection_Outside_UIBase = true,
            Unhollowed_Modules_Folder = Path.Combine(BepInEx.Paths.BepInExRootPath, "interop") // or null
        };
        
        Universe.Init(startupDelay, OnInitialized, LogHandler, config);
    }
    
    static void OnInitialized()
    {
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        UiBase = UniversalUI.RegisterUI(unixTimestamp.ToString(), UiUpdate);
        
        CreateAllPanels();
    }
    
    static void LogHandler(string message, LogType type)
    {
        EasyCards.Log.LogInfo($"UiManager.Log: {message}");
    }

    public static void CreateAllPanels()
    {
        CreateCardPanel();
    }

    private static void CreateCardPanel()
    {
        CardPanel = new CardPanel(UiBase);
        CardPanel.SetActive(false);
    }

    public static void ShowCardPanel()
    {
        CardPanel.SetActive(true);
    }

    public static void HideCardPanel()
    {
        CardPanel.SetActive(false);
    }

    static void UiUpdate()
    {
        // Called once per frame when your UI is being displayed.
    }

}