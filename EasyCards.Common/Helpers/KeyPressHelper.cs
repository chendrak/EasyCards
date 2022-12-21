namespace EasyCards.Common.Helpers;

using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public struct KeyPressData
{
    internal List<Key> KeysToPress;

    public KeyPressData(params Key[] keysToPress)
    {
        this.KeysToPress = keysToPress.ToList();
    }
}

public static class KeyPressHelper
{
    private static Dictionary<KeyPressData, Action> _keypressActions = new();

    static KeyPressHelper()
    {
        UnityUpdateHelper.OnUnityUpdate += OnUpdate;
    }

    public static void RegisterKey(Key key, Action action)
    {
        RegisterKeyCombo(new KeyPressData(key), action);
    }

    public static void RegisterKeyCombo(KeyPressData data, Action action)
    {
        _keypressActions[data] = action;
    }

    private static void OnUpdate()
    {
        var kb = Keyboard.current;

        foreach (var keypressAction in _keypressActions)
        {
            var keys = keypressAction.Key;
            if (keys.KeysToPress.All(key => kb[key].wasPressedThisFrame)) {
                keypressAction.Value.Invoke();
            }
        }
    }
}
