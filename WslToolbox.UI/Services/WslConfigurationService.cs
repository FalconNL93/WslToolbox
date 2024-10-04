using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;
using Microsoft.Extensions.Logging;
using WslToolbox.UI.Core.Configurations;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Services;

public class WslConfigurationService(ILogger<WslConfigurationService> logger)
{
    private readonly string _configPath = Toolbox.WslConfiguration;

    public WslConfigModel GetConfig()
    {
        if (!File.Exists(Toolbox.WslConfiguration))
        {
            return new WslConfigModel();
        }

        var parser = new FileIniDataParser(new IniDataParser(new IniParserConfiguration
        {
            CaseInsensitive = false
        }));

        var data = parser.ReadFile(Toolbox.WslConfiguration);

        var wsl2Section = new Wsl2ConfigSection();
        var wsl2SectionConfig = data.Sections.GetSectionData("wsl2")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);

        if (wsl2SectionConfig != null)
        {
            foreach (var wsl2SectionItem in wsl2SectionConfig)
            {
                var configKey = wsl2Section.Settings.FirstOrDefault(x => string.Equals(x.Key, wsl2SectionItem.Key, StringComparison.CurrentCultureIgnoreCase));
                if (configKey == null)
                {
                    continue;
                }

                configKey.Value = wsl2SectionItem.Value;
            }
        }
        
        return new WslConfigModel
        {
            Wsl2Section = wsl2Section
        };
    }

    private void CreateConfig()
    {
        if (File.Exists(_configPath))
        {
            return;
        }

        logger.LogInformation("WSL Configuration {ConfigFile} does not exist, creating new one", _configPath);
        File.WriteAllText(_configPath, "[wsl2]");
    }

    public void RestoreConfiguration()
    {
        if (!File.Exists(_configPath))
        {
            return;
        }

        File.Delete(_configPath);
    }

    public void WriteConfig(IEnumerable<WslSetting> wslSettings)
    {
        var entries = wslSettings.ToList();
        var configEntries = new Dictionary<string, List<WslSetting>>();

        foreach (var entry in entries)
        {
            var nullOrEmpty = string.IsNullOrEmpty(entry.Value?.ToString());
            var sameAsDefault = string.Equals(entry.Value?.ToString(), entry.Default?.ToString(), StringComparison.CurrentCultureIgnoreCase);
            entry.FlagForRemoval = nullOrEmpty || sameAsDefault;


            if (configEntries.TryGetValue(entry.Section, out var sectionDictVal))
            {
                sectionDictVal.Add(entry);
                continue;
            }

            configEntries.Add(entry.Section, [entry]);
        }

        WriteConfig("wsl2", configEntries);
    }

    public void WriteConfig(string sectionName, string key, string? value)
    {
        if (!File.Exists(_configPath))
        {
            CreateConfig();
        }

        var parser = new FileIniDataParser();
        var data = parser.ReadFile(_configPath);

        if (!data.Sections.ContainsSection(sectionName))
        {
            data.Sections.AddSection(sectionName);
        }

        var section = data.Sections.GetSectionData(sectionName);


        if (string.IsNullOrEmpty(value))
        {
            if (section.Keys.ContainsKey(key))
            {
                section.Keys.RemoveKey(key);
            }
        }
        else
        {
            var keyData = new KeyData(key) {Value = value};
            if (section.Keys.ContainsKey(keyData.KeyName))
            {
                section.Keys.SetKeyData(keyData);
            }
            else
            {
                section.Keys.AddKey(keyData);
            }
        }

        parser.WriteFile(_configPath, data);
    }

    public void WriteConfig(string sectionName, Dictionary<string, List<WslSetting>> values)
    {
        if (!File.Exists(_configPath))
        {
            CreateConfig();
        }

        var parser = new FileIniDataParser();
        var data = parser.ReadFile(_configPath);

        var nodes = values.ToArray();
        foreach (var node in nodes)
        {
            if (!data.Sections.ContainsSection(node.Key))
            {
                data.Sections.AddSection(node.Key);
            }

            var section = data.Sections.GetSectionData(node.Key);
            var items = node.Value.ToArray();

            foreach (var wslSetting in items.Where(x => x.FlagForRemoval))
            {
                if (section.Keys.ContainsKey(wslSetting.Key))
                {
                    section.Keys.RemoveKey(wslSetting.Key);
                }
            }

            foreach (var item in items.Where(x => x is
                     {
                         FlagForRemoval: false,
                         Value: not null
                     }))
            {
                section.Keys[item.Key] = item.Value?.ToString();
            }
        }

        parser.WriteFile(_configPath, data);
    }
}