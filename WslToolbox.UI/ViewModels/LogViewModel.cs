using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Documents;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.ViewModels;

public partial class LogViewModel(ILogger<LogViewModel> logger) : ObservableRecipient
{
    public List<Paragraph> Paragraphs { get; } = [];

    [RelayCommand]
    private void OpenLogFile()
    {
        ShellHelper.OpenFile(Toolbox.LogFile);
    }
}