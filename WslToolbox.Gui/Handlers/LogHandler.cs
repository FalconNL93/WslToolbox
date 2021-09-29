using Serilog;
using Serilog.Core;

namespace WslToolbox.Gui.Handlers
{
    public class LogHandler
    {
        public static Logger Log()
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(Configurations.LogConfiguration.Configuration(), sectionName: "Logging")
                .WriteTo.File(Configurations.LogConfiguration.FileName, shared: true, rollOnFileSizeLimit: true)
                .CreateLogger();
        }
    }
}