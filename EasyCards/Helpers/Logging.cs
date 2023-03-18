namespace EasyCards.Helpers;

using System;
using System.Collections.Generic;
using Serilog;
using SLog = Serilog.Log;

/// <summary>
/// Helper classthat simplifies logging within your plugin.
/// </summary>
public static class Log
{
    static Log()
    {
        SLog.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs\\EasyCards.log")
            .CreateLogger();
    }

    public static void Info(string msg)
    {
        SLog.Information(msg);
    }

    public static void Debug(string msg)
    {
        SLog.Debug(msg);
    }

    public static void Message(string msg)
    {
        SLog.Verbose(msg);
    }

    public static void Error(string msg)
    {
        SLog.Error(msg);
    }

    public static void Warn(string msg)
    {
        SLog.Warning(msg);
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
            values.TryAdd(field.Name, field.GetValue(data));
        });

        Array.ForEach(properties, (property) =>
        {
            values.TryAdd(property.Name, property.GetValue(data));
        });

        var lines = new List<string>();
        foreach (var value in values)
        {
            lines.Add($"\"{value.Key}\":\"{value.Value}\"");
        }

        return $"\"{type}\": " + "{" + String.Join(",", lines) + "}";
    }
}
