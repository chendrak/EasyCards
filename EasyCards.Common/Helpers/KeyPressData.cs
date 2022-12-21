namespace EasyCards.Common.Helpers;

using UnityEngine.InputSystem;

public struct KeyPressData
{
    internal Key Key;
    internal Modifiers Modifiers;

    public KeyPressData(Key keyToPress, Modifiers modifiers = Modifiers.None)
    {
        this.Key = keyToPress;
        this.Modifiers = modifiers;
    }
}
