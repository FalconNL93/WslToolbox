using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public partial class UpdateResultModel : ObservableRecipient
{
    public readonly Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

    [ObservableProperty]
    private Uri _downloadUri;

    [ObservableProperty]
    private DateTime _lastChecked;

    [ObservableProperty]
    private Version _latestVersion = Assembly.GetExecutingAssembly().GetName().Version;

    [ObservableProperty]
    private string _updateStatus;

    public bool UpdateAvailable => LatestVersion.CompareTo(CurrentVersion) > 0;
}