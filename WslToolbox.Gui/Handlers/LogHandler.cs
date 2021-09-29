using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using System;
using System.IO;
using System.Threading;

namespace WslToolbox.Gui.Handlers
{
    public class LogHandler
    {
        public static Logger Log()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(Configurations.LogConfiguration.Configuration())
                .WriteTo.File(Configurations.LogConfiguration.FileName, shared: true, rollOnFileSizeLimit: true)
                .CreateLogger();
        }


    }
}
