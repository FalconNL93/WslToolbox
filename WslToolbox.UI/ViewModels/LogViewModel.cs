using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public partial class LogViewModel : ObservableRecipient
{
    private readonly LogService _logService;

    public LogViewModel(LogService logService)
    {
        _logService = logService;
    }

    public static async Task ReadLog(CancellationTokenSource cancellationToken)
    {
        await LogService.ReadLog(cancellationToken);
    }
}