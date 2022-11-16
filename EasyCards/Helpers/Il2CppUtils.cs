namespace EasyCards.Helpers;

using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;

public static class Il2CppUtils
{
    public static T NewILObjectInstance<T>() where T : Il2CppObjectBase
    {
        var ptr = ClassInjector.DerivedConstructorPointer<T>();
        return new Il2CppObjectBase(ptr).Cast<T>();
    }
}
