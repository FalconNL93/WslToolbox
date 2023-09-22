using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;
using WslToolbox.UI.Models;
using static System.Boolean;

namespace WslToolbox.UI.Helpers;

public static class WslConfigHelper
{
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

        var experimentalKeys = data.Sections.GetSectionData("experimental").Keys;
        TryParse(experimentalKeys.FirstOrDefault(x => x.KeyName == "sparseVhd")?.Value, out var sparseVhdBool);

        return new WslConfigModel
        {
            Boot = new BootConfig
            {
                Systemd = null
            },
            Experimental = new ExperimentalConfig
            {
                SparseVhd = sparseVhdBool,
                AutoMemoryReclaim = null
            }
        };
    }
}