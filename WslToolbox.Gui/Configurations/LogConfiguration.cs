using Microsoft.Extensions.Configuration;
using System.IO;

namespace WslToolbox.Gui.Configurations
{
    public class LogConfiguration
    {
        public static string FileName = $"log.txt";

        public static IConfiguration Configuration()
        {
            return new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile(path: "settings.json", optional: true, reloadOnChange: true)
                 .Build();
        }

        public enum LogLevel
        {
            Verbose,
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }
    }
}