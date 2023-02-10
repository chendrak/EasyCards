namespace EasyCards.Common.Helpers;

using System;
using System.Collections.Generic;

public class LoggingHelper
{
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
