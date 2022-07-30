using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace WslToolbox.Gui2.Extensions;

public static class SaveConfigurationExtension
{
    public const string FileName = "appsettings.json";

    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public static void Save<T>(this IOptions<T> options) where T : class
    {
        var jsonString = JsonSerializer.Serialize(
            new Dictionary<string, T> {{typeof(T).Name, options.Value}},
            Options);

        File.WriteAllText(FileName, jsonString);
    }
}