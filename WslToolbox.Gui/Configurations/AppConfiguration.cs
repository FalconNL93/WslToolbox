using System.IO;
using System.Reflection;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public static class AppConfiguration
    {
        public static string AppName => "WSL Toolbox";
        public static string AppLogsFileName => "logs.txt";
        public static string DatabaseFilename => "wsltoolbox.db";
        public static bool EnableUpdater => true;
        public static string GithubRepository => "https://github.com/FalconNL93/WslToolbox";
        public static string GithubDocs => "https://falconnl93.github.io/wsltoolbox-docs";

#if(DEBUG)
        //public static string AppConfigurationUpdateXml => "http://localhost/wsltoolbox.xml";
        public static string AppConfigurationUpdateXml => "https://falconnl93.github.io/wsltoolbox-docs/update.xml";
        public static bool DebugMode => true;
        public static string AppConfigurationFileName => "appsettings-dev.json";
#else
        public static bool DebugMode => false;
        public static string AppConfigurationUpdateXml => "https://falconnl93.github.io/wsltoolbox-docs/update.xml";
        public static string AppConfigurationFileName => "appsettings.json";
#endif

        public static string AppExecutableDirectory { get; } =
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static LogEventLevel AppDefaultLogLevel()
        {
            return LogEventLevel.Error;
        }
    }
}