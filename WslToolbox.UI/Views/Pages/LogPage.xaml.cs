using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Navigation;
using WslToolbox.UI.Core.Sinks;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class LogPage : Page
{
    public LogPage()
    {
        ViewModel = App.GetService<LogViewModel>();
        InitializeComponent();

        Debug.WriteLine("Create logging event listener");
        EventSink.NewLogHandler -= LogServiceOnLogFileChanged;
        EventSink.NewLogHandler += LogServiceOnLogFileChanged;
    }

    private void LogServiceOnLogFileChanged(object? sender, LogEventArgs e)
    {
        WriteUiLog(e.Log.RenderMessage());
    }

    private void WriteUiLog(string line)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run {Text = line});
            LogBlock.Blocks.Add(paragraph);
        });
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }

    [RelayCommand]
    private void ClearLog()
    {
        LogBlock.Blocks.Clear();
    }

    public LogViewModel ViewModel { get; }
}