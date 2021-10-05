using System.Configuration;
using System.IO;
using System.Reflection;
using Serilog.Events;
using WslToolbox.Gui.Annotations;

namespace WslToolbox.Gui.Configurations
{
    public static class AppConfiguration
    {
        public static bool IsDebugRelease
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public static string AppName { get; } = Get("AppName");
        public static string AppLogsDirectory { get; } = Get("AppLogsDirectory");
        public static string AppLogsFileName { get; } = Get("AppLogsFileName");
        public static string AppConfigurationFileName { get; } = Get("AppConfigurationFileName");

        public static string AppExecutableDirectory { get; } =
            Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static int AppMinimalOsBuild { get; } = int.Parse(Get("AppMinimalOsBuild"));

        [CanBeNull]
        private static string Get(string configKey)
        {
            return ConfigurationManager.AppSettings.Get(configKey);
        }

        public static LogEventLevel AppDefaultLogLevel()
        {
            return IsDebugRelease ? LogEventLevel.Debug : LogEventLevel.Error;
        }
    }
}