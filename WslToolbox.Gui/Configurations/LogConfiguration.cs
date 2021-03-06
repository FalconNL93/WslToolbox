using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public static class LogConfiguration
    {
        [JsonIgnore] public static readonly string FileName =
            $"{AppConfiguration.AppExecutableDirectory}/{AppConfiguration.AppLogsFileName}";

        public static LogEventLevel MinimumLevel { get; set; } = AppConfiguration.AppDefaultLogLevel();

        public static IConfiguration Configuration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppConfiguration.AppConfigurationFileName}", true, true)
                .Build();
        }

        public static IEnumerable<LogEventLevel> GetValues()
        {
            return Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>();
        }
    }
}