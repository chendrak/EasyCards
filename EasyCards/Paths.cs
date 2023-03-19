namespace EasyCards;

using System.IO;

public static class Paths
{
    public static string EasyCards { get; private set; }
    public static string Assets { get; private set; }
    public static string Data { get; private set; }
    public static string BaseModDirectory { get; private set; }

    public static void Initialize(DirectoryInfo modDirectory)
    {
        EasyCards = modDirectory.FullName;
        Assets = Path.Combine(EasyCards, "Assets");
        Data = Path.Combine(EasyCards, "Data");
        BaseModDirectory = modDirectory.Parent.FullName;
    }
}
