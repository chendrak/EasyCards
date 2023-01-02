namespace EasyCards.Common.Extensions;

using System.Reflection;

public static class ClassExtensions
{
    public static ConstructorInfo? GetEmptyCtor(this Type type) => type.GetConstructor(Type.EmptyTypes);
}
