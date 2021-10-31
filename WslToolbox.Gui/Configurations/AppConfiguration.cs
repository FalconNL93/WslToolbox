using System.IO;
using System.Reflection;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public static class AppConfiguration
    {
        public static string AppName => "WSL Toolbox";
        public static string AppLogsFileName => "logs.txt";
        public static string AppConfigurationFileName => "settings.json";

        public static string AppExecutableDirectory { get; } =
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static LogEventLevel AppDefaultLogLevel()
        {
            return LogEventLevel.Error;
        }
    }
}