using System;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class OsHandler
    {
        public static bool Supported()
        {
            return Environment.OSVersion.Version.Build >= AppConfiguration.AppMinimalOsBuild;
        }
    }
}