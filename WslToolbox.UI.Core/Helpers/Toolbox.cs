using System.Reflection;

namespace WslToolbox.UI.Core.Helpers;

public static class Toolbox
{
    public static readonly string CpuInfo = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
    public static readonly string ProcessType = Environment.Is64BitProcess ? "x64" : "x86";

    public static readonly string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
    public static readonly string AppData = @$"{AppDirectory}\data";

    public static readonly string UserConfiguration = @$"{AppData}\config.json";
    public static readonly string LogFile = @$"{AppData}\log.txt";
    public static readonly string StoreUrl = "ms-windows-store://pdp/?productid=9NDGGX7M2H0V";
}