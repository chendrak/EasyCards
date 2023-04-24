namespace EasyCards.Common.Logging;

using System;
using System.Collections.Generic;
using SLog = UnityEngine.Debug;

/// <summary>
/// Helper class that simplifies logging within your plugin.
/// </summary>
public static class Log
{
    private static string Prefix = "CustomMod";
    public enum LogLevel
    {
        ERROR,
        WARN,
        INFO,
        DEBUG,
        VERBOSE
    }

    public static void Initialize(string prefix) => Prefix = prefix;

    private static LogLevel logLevel = LogLevel.INFO;

    public static void SetMinimumLogLevel(LogLevel logLevel)
    {
        Log.logLevel = logLevel;
    }

    public static void Info(string msg)
    {
        if (logLevel >= LogLevel.INFO)
        {
            SLog.Log($"[{Prefix}] [INFO] {msg}");
        }
    }

    public static void Debug(string msg)
    {
        if (logLevel >= LogLevel.DEBUG)
        {
            SLog.Log($"[{Prefix}] [DEBUG] {msg}");
        }
    }

    public static void Error(string msg)
    {
        SLog.LogError($"[{Prefix}] [ERROR] {msg}");
    }

    public static void Verbose(string msg)
    {
        if (logLevel >= LogLevel.VERBOSE)
        {
            SLog.Log($"[{Prefix}] [VERBOSE] {msg}");
        }
    }

    public static void Warn(string msg)
    {
        if (logLevel >= LogLevel.WARN)
        {
            SLog.LogWarning($"[{Prefix}] [WARN] {msg}");
        }
    }

    /// <summary>
    /// A method that attempts to read through all fields and properties of the provided type and return
    /// a string that contains all that data.
    /// </summary>
    public static string StructToString<T>(T data)
    {
        var type = data.GetType();
        var fields = type.GetFields();
        var properties = type.GetProperties();

        var values = new Dictionary<string, object>();
        Array.ForEach(fields, (field) =>
        {
            values.Add(field.Name, field.GetValue(data));
        });

        Array.ForEach(properties, (property) =>
        {
            values.Add(property.Name, property.GetValue(data));
        });

        var lines = new List<string>();
        foreach (var value in values)
        {
            lines.Add($"\"{value.Key}\":\"{value.Value}\"");
        }

        return $"\"{type}\": " + "{" + String.Join(",", lines) + "}";
    }
}
