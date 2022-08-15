using System.Diagnostics;

namespace WslToolbox.Gui2.Helpers;

public static class FileHelper
{
    public static void OpenFile(string path)
    {
        var logOpener = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = "\"" + $"{path}" + "\""
            }
        };

        logOpener.Start();
    }
}