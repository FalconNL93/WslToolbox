using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;
using WslToolbox.Core.Legacy.Helpers;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Helpers;

public static class WslConfigHelper
{
    public static Dictionary<string, string> NetworkingModeList { get; set; } = new()
    {
        {"", "Default"},
        {"NAT", "NAT"},
        {"mirrored", "mirrored "}
    };

    public static WslConfigModel GetConfig()
    {
        var wslConfigFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.wslconfig";

        if (!File.Exists(wslConfigFile))
        {
            return new WslConfigModel();
        }

        var parser = new FileIniDataParser(new IniDataParser(new IniParserConfiguration
        {
            CaseInsensitive = false,
        }));
        var data = parser.ReadFile(wslConfigFile);


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

    public static void WriteConfig(string sectionName, string key, string? value)
    {
        var wslConfig = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wslconfig");
        var parser = new FileIniDataParser();
        var data = parser.ReadFile(wslConfig);

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

        parser.WriteFile(wslConfig, data);
    }
}