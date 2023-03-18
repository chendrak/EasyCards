namespace EasyCards;

public static class Paths
{
    public static string EasyCards = Path.Combine(BepInEx.Paths.PluginPath, MyPluginInfo.PLUGIN_NAME);
    public static string Assets = Path.Combine(EasyCards, "Assets");
    public static string Data = Path.Combine(EasyCards, "Data");
    public static string Plugins = BepInEx.Paths.PluginPath;
}
