using System.IO;
using System.Reflection;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public static class AppConfiguration
    {
        public static string AppLogsFileName => "logs.txt";
        public static string AppConfigurationFileName => "settings.json";
        public static bool EnableUpdater => true;
#if(DEBUG)
        //public static string AppConfigurationUpdateXml => "http://localhost/wsltoolbox.xml";
        public static string AppName => "WSL Toolbox (Dev)";
        public static string AppConfigurationUpdateXml => "https://falconnl93.github.io/wsltoolbox-docs/update.xml";
        public static bool DebugMode => true;
#else
        public static bool DebugMode => false;
        public static string AppName => "WSL Toolbox";
        public static string AppConfigurationUpdateXml => "https://falconnl93.github.io/wsltoolbox-docs/update.xml";
#endif

        public static string AppExecutableDirectory { get; } =
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static LogEventLevel AppDefaultLogLevel()
        {
            return LogEventLevel.Error;
        }
    }
}