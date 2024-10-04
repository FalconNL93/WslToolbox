using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace WslToolbox.Cli;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = matchArgs => matchArgs.Header.ToLower(),
            Delimiter = ";"
        };
        using var reader = new StreamReader("C:\\Users\\pvand\\Downloads\\wsl2.csv");
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<ConfigImport>();
        var configImports = records.ToList();
        var allRecords = configImports.ToList();

        Console.WriteLine($"Number of records: {allRecords.Count()}");


        var folder = "C:\\Users\\pvand\\Downloads\\";
        var file = "writer.txt";
        var filePath = Path.Combine(folder, file);

        File.Delete(filePath);

        await using var outputFile = new StreamWriter(filePath, true);
        foreach (var record in allRecords)
        {
            var recordValue = record.Value;
            var defaultValueFile = record.Default.ToLower();
            var note = record.Notes.Replace("\"", "");
            var defaultValue = "null";

            if (record.Value == "boolean")
            {
                defaultValue = record.Default.ToLower();
            }
            else if (!defaultValueFile.Contains(' '))
            {
                defaultValue = $@"@""{record.Default}""";
            }
            else if (defaultValueFile == "blank")
            {
                defaultValue = $@"""{string.Empty}""";
            }

            var ruleWithValue = $@"new WslSetting
        {{
            Section = ""wsl2"",
            Key = ""{record.Key}"",
            Value = {defaultValue},
            Default = {defaultValue},
            Description = ""{note}"",
        }},";

            var ruleWithoutValue = $@"new WslSetting
        {{
            Section = ""wsl2"",
            Key = ""{record.Key}"",
            Default = {defaultValue},
            Description = ""{note}"",
        }},";

            await outputFile.WriteAsync(ruleWithoutValue);
        }
    }
}