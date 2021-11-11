using System.IO;
using System.Reflection;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public static class AppConfiguration
    {
        public static string AppName => "WSL Toolbox - Beta";
        public static string AppLogsFileName => "logs.txt";
        public static string AppConfigurationFileName => "settings.json";
#if(DEBUG)
        public static string AppConfigurationUpdateXml => "http://localhost/wsltoolbox.xml";
#else
        public static string AppConfigurationUpdateXml => null;
#endif

        public static string AppExecutableDirectory { get; } =
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static LogEventLevel AppDefaultLogLevel()
        {
            return LogEventLevel.Error;
        }
    }
}