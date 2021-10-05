using System.IO;
using Microsoft.Extensions.Configuration;

namespace WslToolbox.Gui.Configurations
{
    public static class LogConfiguration
    {
        public static readonly string FileName =
            $"{AppConfiguration.AppExecutableDirectory}/{AppConfiguration.AppLogsFileName}";

        public static IConfiguration Configuration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppConfiguration.AppConfigurationFileName}", true, true)
                .Build();
        }
    }
}