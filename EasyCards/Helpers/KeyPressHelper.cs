namespace EasyCards.Helpers;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Flags]
public enum Modifiers
{
    None = 0,
    Shift = 1,
    Ctrl = 2,
    Alt = 4,
}

public static class KeyPressHelper
{
    private static GameObject managerGo;
    private static KeyPressBehaviour _behaviour;

    static KeyPressHelper()
    {
        managerGo = new GameObject { hideFlags = HideFlags.HideAndDontSave };
        _behaviour = managerGo.AddComponent<KeyPressBehaviour>();
    }

    public static void RegisterKey(Key key, Modifiers modifiers, Action action)
    {
        _behaviour.RegisterKeyCombo(key, modifiers, action);
    }
}

class KeyPressBehaviour : MonoBehaviour
{
    internal static Dictionary<KeyPressData, Action> _keypressActions = new();

    internal void RegisterKeyCombo(Key key, Modifiers modifiers, Action action)
    {
        var data = new KeyPressData(key, modifiers);
        _keypressActions[data] = action;
    }

    private void FixedUpdate()
    {
        var kb = Keyboard.current;

        foreach (var keypressAction in _keypressActions)
        {
            var data = keypressAction.Key;
            if (kb[data.Key].wasPressedThisFrame && this.AreModifiersHeld(data.Modifiers))
            {
                keypressAction.Value.Invoke();
            }
        }
    }

    private bool AreModifiersHeld(Modifiers modifiers)
    {
        var kb = Keyboard.current;
        if ((modifiers & Modifiers.Shift) == Modifiers.Shift && !kb.shiftKey.isPressed)
            return false;
        if ((modifiers & Modifiers.Ctrl) == Modifiers.Ctrl && !kb.ctrlKey.isPressed)
            return false;
        if ((modifiers & Modifiers.Alt) == Modifiers.Alt && !kb.altKey.isPressed)
            return false;
        return true;
    }
}
