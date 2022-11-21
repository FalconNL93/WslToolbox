using System.Diagnostics;
using WslToolbox.UI.Core.Models;

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
    
    public static void StartWebAdmin(WebAdminModel webAdminModel)
    {
        if (!File.Exists("WslToolbox.Web.exe"))
        {
            return;
        }
        
        using var webAdmin = new Process();
        webAdmin.StartInfo.FileName = "WslToolbox.Web.exe";
        webAdmin.StartInfo.Arguments = "";
        webAdmin.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        webAdmin.Start();
    }
}