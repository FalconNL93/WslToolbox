using System.Diagnostics;
using WslToolbox.UI.Core.EventArguments;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Core.Services;

public class LogService
{
    public static event EventHandler<LogFileChangedEventArgs> LogFileChanged;

    public LogService()
    {
    }

    public static async Task ReadLog(CancellationTokenSource cancellationToken)
    {
        await FetchLog(cancellationToken);
        
        var watcher = new FileSystemWatcher
        {
            Path = Toolbox.AppData,
            Filter = "blaat.txt",
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true,
        };

        watcher.Changed += async (_, _) => await FetchLog(cancellationToken);
    }

    private static async Task FetchLog(CancellationTokenSource cancellationToken)
    {
        await using var fileStream = File.Open("data\\blaat.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        var line = await reader.ReadLineAsync(cancellationToken.Token);
        if (string.IsNullOrWhiteSpace(line))
        {
            return;
        }

        Debug.WriteLine(line);
        LogFileChanged?.Invoke(nameof(LogService), new LogFileChangedEventArgs(line));
    }
}