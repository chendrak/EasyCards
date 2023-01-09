namespace EasyCards.Common.Extensions;

using System.Reflection;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

public static class ClassExtensions
{
    public static ConstructorInfo? GetEmptyCtor(this Type type) => type.GetConstructor(Type.EmptyTypes);
    public static Il2CppSystem.Reflection.ConstructorInfo? GetEmptyIl2CppCtor(this Type type) => Il2CppType.From(type).GetConstructor(new Il2CppReferenceArray<Il2CppSystem.Type>(0));
}
