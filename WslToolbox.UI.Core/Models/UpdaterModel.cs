using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public class UpdateResultModel : ObservableRecipient
{
    public DateTime LastSearched { get; set; }
    public readonly Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
    private string _updateStatus;

    public string UpdateStatus
    {
        get => _updateStatus;
        set => SetProperty(ref _updateStatus, value);
    }

    public bool UpdateAvailable => LatestVersion.CompareTo(CurrentVersion) > 0;
    public Uri DownloadUri { get; set; }
    public Version LatestVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version;
}