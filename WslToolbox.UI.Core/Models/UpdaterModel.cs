using CommunityToolkit.Mvvm.ComponentModel;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Core.Models;

public partial class UpdateResultModel : ObservableRecipient
{
    public readonly Version CurrentVersion = Toolbox.Version;

    [ObservableProperty]
    private Uri _downloadUri;

    [ObservableProperty]
    private DateTime _lastChecked;

    [ObservableProperty]
    private Version _latestVersion = Toolbox.Version;

    [ObservableProperty]
    private string _updateStatus;

    [ObservableProperty]
    private bool _isChecking;

    public bool UpdateAvailable => LatestVersion.CompareTo(CurrentVersion) > 0;
}