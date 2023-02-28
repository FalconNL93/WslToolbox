using System.Diagnostics;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WslToolbox.UI.Core.EventArguments;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class LogPage : Page
{
    private readonly CancellationTokenSource _cancellationToken = new();
    private int _logChanged = 0;
    public LogPage()
    {
        ViewModel = App.GetService<LogViewModel>();
        InitializeComponent();
        
        LogService.LogFileChanged += LogServiceOnLogFileChanged;
    }

    private void LogServiceOnLogFileChanged(object? sender, LogFileChangedEventArgs e)
    {
        _logChanged++;
        Debug.WriteLine($"Log changed {_logChanged}");
        
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
        {
            LogBlock.Text = $"{e.LogEntry}{Environment.NewLine}{Environment.NewLine}";
        });
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await LogViewModel.ReadLog(_cancellationToken);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _cancellationToken.Cancel();
    }

    public LogViewModel ViewModel { get; }
}