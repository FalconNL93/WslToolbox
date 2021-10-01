using System.IO;
using Microsoft.Extensions.Configuration;

namespace WslToolbox.Gui.Configurations
{
    public static class LogConfiguration
    {
        public static readonly string FileName = $"{Directory.GetCurrentDirectory()}/logs/log.txt";

        public static IConfiguration Configuration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", true, true)
                .Build();
        }
    }
}