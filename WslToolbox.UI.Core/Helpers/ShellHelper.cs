using System.Diagnostics;

namespace WslToolbox.UI.Core.Helpers;

public static class ShellHelper
{
    public static void OpenFile(string path)
    {
        using var process = new Process();
        process.StartInfo.FileName = "explorer";
        process.StartInfo.Arguments = "\"" + path + "\"";
        process.Start();
    }
}