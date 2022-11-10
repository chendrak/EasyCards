using BepInEx.Configuration;
using Microsoft.Extensions.Logging;

namespace EasyCards.Logging;

public sealed class LoggerConfiguration : ILoggerConfiguration
{
    private readonly ConfigEntry<bool> _logCardDebuggingConfigEntry;

    public LoggerConfiguration(ConfigFile configFile)
    {
        _logCardDebuggingConfigEntry = configFile.Bind("Debug", "LogCards", false,
            "Allows for card details to be logged");
    }

    public bool IsLoggerEnabled() => this._logCardDebuggingConfigEntry.Value;

    public bool IsLoggerEnabled(string category) => this.IsLoggerEnabled();

    public bool IsLogLevelEnabled(string category, LogLevel logLevel) => IsLoggerEnabled(category) && (logLevel > LogLevel.Debug || _logCardDebuggingConfigEntry.Value);
}
