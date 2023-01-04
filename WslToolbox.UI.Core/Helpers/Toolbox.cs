using System.Reflection;

namespace WslToolbox.UI.Core.Helpers;

public static class Toolbox
{
    public enum AppTypes
    {
        Portable,
        Setup
    }

    public static readonly string CpuInfo = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
    public static readonly string ProcessType = Environment.Is64BitProcess ? "x64" : "x86";

    public static readonly string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
    public static readonly string AppData = Path.Combine(AppDirectory, "data");

    public static readonly string UserConfiguration = Path.Combine(AppData, "config.json");
    public static readonly string LogFile = Path.Combine(AppData, "log.txt");
    public static readonly Uri GitHubDownloadUrl = new("https://github.com/FalconNL93/WslToolbox/releases/download/");
    public static readonly Uri GitHubManifestFile = new("https://raw.githubusercontent.com/FalconNL93/manifests/main/");
    public static readonly string StoreUrl = "ms-windows-store://pdp/?productid=9NDGGX7M2H0V";

    public static AppTypes GetAppType()
    {
        return File.Exists(Path.Combine(AppDirectory, "unins001.exe"))
            ? AppTypes.Setup
            : AppTypes.Portable;
    }
}