using Serilog;
using Serilog.Core;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class LogHandler
    {
        public static Logger Log()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(LogConfiguration.Configuration(), "Logging")
                .WriteTo.File(LogConfiguration.FileName, shared: true, rollOnFileSizeLimit: true)
                .CreateLogger();
        }
    }
}