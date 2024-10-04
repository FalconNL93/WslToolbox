using System.Reflection;
using Serilog;

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

    public static readonly string AppInstallDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
    public static readonly string AppDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wsltoolbox");
    public static readonly string AppData = Path.Combine(AppDirectory);

    public static readonly string UserConfiguration = Path.Combine(AppData, "config.json");
    public static readonly string LogConfiguration = Path.Combine(AppData, "log.json");
    public static readonly string LogFile = Path.Combine(AppData, "log.txt");
    public static readonly Uri GitHubDownloadUrl = new("https://github.com/FalconNL93/WslToolbox/releases/download/");
    public static readonly Uri GitHubManifestFile = new("https://raw.githubusercontent.com/FalconNL93/manifests/main/");
    public static readonly string StoreUrl = "ms-windows-store://pdp/?productid=9NDGGX7M2H0V";

    public static readonly string WslConfiguration = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\.wslconfig";

    public static AppTypes GetAppType()
    {
        return File.Exists(Path.Combine(AppInstallDir, "unins001.exe"))
            ? AppTypes.Setup
            : AppTypes.Portable;
    }

    public static void CopyOldConfiguration()
    {
        var oldDirectory = Path.Combine(AppInstallDir, "data");
        var newDirectory = AppDirectory;
        if (!Directory.Exists(oldDirectory))
        {
            return;
        }

        if (File.Exists(Toolbox.UserConfiguration))
        {
            Log.Logger.Information("User configuration already exist");

            return;
        }
        

        try
        {
            DirectoryHelper.CopyDirectory(oldDirectory, AppDirectory, true);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Unable to copy configuration files to new directory");
        }
    }
}