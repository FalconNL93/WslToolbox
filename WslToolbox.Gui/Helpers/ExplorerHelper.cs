using System;
using System.Diagnostics;
using System.Windows.Documents;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Helpers
{
    public static class ExplorerHelper
    {
        public static void OpenHyperlink(Hyperlink hyperlink)
        {
            hyperlink.RequestNavigate += (sender, e) =>
            {
                try
                {
                    _ = Process.Start(new ProcessStartInfo("explorer")
                    {
                        Arguments = e.Uri.ToString()
                    });
                }
                catch (Exception ex)
                {
                    LogHandler.Log().Error(ex, ex.Message);
                }
            };
        }

        public static void OpenLocal(string path)
        {
            try
            {
                _ = Process.Start(new ProcessStartInfo("explorer")
                {
                    Arguments = path
                });
            }
            catch (Exception ex)
            {
                LogHandler.Log().Error(ex, ex.Message);
            }
        }
    }
}