using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;
using WslToolbox.Core.Legacy.Helpers;
using WslToolbox.UI.Models;

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


        var bootKeys = data.Sections.GetSectionData("boot").Keys.ToDictionary(x => x.KeyName, x => x.Value);
        var experimentalKeys = data.Sections.GetSectionData("experimental").Keys.ToDictionary(x => x.KeyName, x => x.Value);
        
        var interopKeys = data.Sections.GetSectionData("interop")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);
        var networkKeys = data.Sections.GetSectionData("network")?.Keys.ToDictionary(x => x.KeyName, x => x.Value);

        var bootSection = InstanceHelper.Create<BootSection>(bootKeys);
        var experimentalSection = InstanceHelper.Create<ExperimentalSection>(experimentalKeys);
        var interopSection = InstanceHelper.Create<InteropSection>(interopKeys);
        var networkSection = InstanceHelper.Create<NetworkSection>(networkKeys);

        return new WslConfigModel
        {
            Boot = bootSection,
            Experimental = experimentalSection,
            Interop = interopSection,
            Network = networkSection
        };
    }
}