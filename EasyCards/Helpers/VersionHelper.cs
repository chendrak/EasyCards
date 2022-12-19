namespace EasyCards.Helpers;

using SemanticVersioning;
using UnityEngine;

public static class VersionHelper
{
    public static readonly Version GameVersion = Version.Parse(Application.version, loose: true);

    public static bool IsGameVersionAtLeast(Version versionToTest) => GameVersion >= versionToTest;
}
