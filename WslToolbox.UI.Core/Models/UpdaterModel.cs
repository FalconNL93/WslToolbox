using CommunityToolkit.Mvvm.ComponentModel;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models.Responses;

namespace WslToolbox.UI.Core.Models;

public partial class UpdateResultModel : ObservableRecipient
{
    public readonly Version CurrentVersion = Toolbox.Version;

    [ObservableProperty]
    private Uri _downloadUri;

    [ObservableProperty]
    private FilesModel _files;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private bool _isChecking;

    [ObservableProperty]
    private DateTime _lastChecked;

    [ObservableProperty]
    private Version _latestVersion = Toolbox.Version;

    [ObservableProperty]
    private string _updateStatus;
    
    [ObservableProperty]
    private string _releaseNotes;

    public bool UpdateAvailable => LatestVersion.CompareTo(CurrentVersion) > 0;
}