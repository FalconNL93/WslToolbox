using System;
using Serilog.Core;
using Serilog;
using System.Reflection;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

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
