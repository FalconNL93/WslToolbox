using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;
using Microsoft.Extensions.Logging;
using WslToolbox.Core.Legacy.Helpers;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Services;

public class WslConfigurationService(ILogger<WslConfigurationService> logger)
{
    private readonly string _configPath = Toolbox.WslConfiguration;

    public static Dictionary<string, string> NetworkingModeList { get; set; } = new()
    {
        {"", "Default"},
        {"NAT", "NAT"},
        {"mirrored", "mirrored "}
    };

    public WslConfigModel GetConfig()
    {
        if (!File.Exists(Toolbox.WslConfiguration))
        {
            return new WslConfigModel();
        }

        var parser = new FileIniDataParser(new IniDataParser(new IniParserConfiguration
        {
            CaseInsensitive = false,
        }));

        var data = parser.ReadFile(Toolbox.WslConfiguration);

        var bootKeys = data.Sections.GetSectionData("boot")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);
        var experimentalKeys = data.Sections.GetSectionData("experimental")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);

        var rootKeys = data.Sections.GetSectionData("wsl2")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);
        var interopKeys = data.Sections.GetSectionData("interop")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);
        var networkKeys = data.Sections.GetSectionData("network")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);

        var rootSection = InstanceHelper.Create<RootSection>(rootKeys);
        var bootSection = InstanceHelper.Create<BootSection>(bootKeys);
        var experimentalSection = InstanceHelper.Create<ExperimentalSection>(experimentalKeys);
        var interopSection = InstanceHelper.Create<InteropSection>(interopKeys);
        var networkSection = InstanceHelper.Create<NetworkSection>(networkKeys);

        return new WslConfigModel
        {
            Root = rootSection,
            Boot = bootSection,
            Experimental = experimentalSection,
            Interop = interopSection,
            Network = networkSection
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
}