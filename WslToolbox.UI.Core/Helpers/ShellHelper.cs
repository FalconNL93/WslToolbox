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

    public static void OpenExecutable(string path, List<string> arguments = null, bool exitApp = false)
    {
        using var process = new Process();
        process.StartInfo.FileName = path;
        
        if (arguments != null && arguments.Any())
        {
            process.StartInfo.Arguments = arguments.Aggregate("", (current, argument) => current + $"{argument} ");
        }

        process.StartInfo.UseShellExecute = false;
        process.Start();

        if (exitApp)
        {
            Environment.Exit(0);
        }
    }

    public static void OpenUrl(Uri url)
    {
        using var process = new Process();
        process.StartInfo.FileName = "explorer";
        process.StartInfo.Arguments = "\"" + url.AbsoluteUri + "\"";
        process.Start();
    }
}