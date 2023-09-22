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

        var configInstance = Activator.CreateInstance(typeof(WslConfigModel));
        foreach (var propertyInfo in configInstance.GetType().GetProperties())
        {
            Console.WriteLine(propertyInfo.Name);
            if (propertyInfo.PropertyType.IsClass)
            {
                Console.WriteLine($"Searching for {propertyInfo.Name.ToLower()}");
                var sectionKeys = data.Sections.GetSectionData(propertyInfo.Name.ToLower());
                if (sectionKeys == null)
                {
                    Console.WriteLine($"Not present");
                    continue;
                }

                var propertyInstance = Activator.CreateInstance(propertyInfo.PropertyType);
                if (propertyInstance == null)
                {
                    continue;
                }
                
                Console.WriteLine($"Created instance: {propertyInstance.GetType()}");
                foreach (var subPropertyInfo in propertyInstance.GetType().GetProperties())
                {
                    object? keyVal = null;
                    if (subPropertyInfo.PropertyType == typeof(bool))
                    {
                        keyVal = false;
                    } else if (subPropertyInfo.PropertyType == typeof(string))
                    {
                        keyVal = "wtf";
                    }
                    
                    subPropertyInfo.SetValue(propertyInstance, keyVal);
                }
                
                Console.WriteLine($"Setting {propertyInfo.GetType()} with {instance.GetType()}");
                propertyInfo.SetValue(configInstance, instance);
            }
        }

        var experimentalKeys = data.Sections.GetSectionData("experimental").Keys;
        TryParse(experimentalKeys.FirstOrDefault(x => x.KeyName == "sparseVhd")?.Value, out var sparseVhdBool);

        return new WslConfigModel
        {
            Boot = new BootSection
            {
                Systemd = null
            },
            Experimental = new ExperimentalSection
            {
                SparseVhd = sparseVhdBool,
                AutoMemoryReclaim = null
            }
        };
    }
}