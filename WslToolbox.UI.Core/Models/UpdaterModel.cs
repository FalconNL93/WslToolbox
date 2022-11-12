using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public class UpdateResultModel : ObservableRecipient
{
    public readonly Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
    private Uri _downloadUri;
    private DateTime _lastChecked;
    private Version _latestVersion = Assembly.GetExecutingAssembly().GetName().Version;
    private string _updateStatus;

    public DateTime LastChecked
    {
        get => _lastChecked;
        set => SetProperty(ref _lastChecked, value);
    }

    public string UpdateStatus
    {
        get => _updateStatus;
        set => SetProperty(ref _updateStatus, value);
    }

    public bool UpdateAvailable => LatestVersion.CompareTo(CurrentVersion) > 0;

    public Uri DownloadUri
    {
        get => _downloadUri;
        set => SetProperty(ref _downloadUri, value);
    }

    public Version LatestVersion
    {
        get => _latestVersion;
        set
        {
            if (SetProperty(ref _latestVersion, value))
            {
                OnPropertyChanged(nameof(UpdateAvailable));
            }
        }
    }
}