using System;
using System.Diagnostics;
using System.Windows.Documents;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Helpers
{
    public static class ShellHelper
    {
        private const string DefaultShell = "explorer";

        public static void OpenHyperlink(Hyperlink hyperlink)
        {
            hyperlink.RequestNavigate += (sender, e) =>
            {
                try
                {
                    _ = Process.Start(new ProcessStartInfo(DefaultShell)
                    {
                        Arguments = e.Uri.ToString()
                    });
                }
                catch (Exception ex)
                {
                    LogHandler.Log().Error(ex, "Could not open URL");
                }
            };
        }

        public static void OpenLocal(string path)
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo(DefaultShell)
                {
                    Arguments = path
                });
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error(ex, "Could not open URL");
            }
        }
    }
}