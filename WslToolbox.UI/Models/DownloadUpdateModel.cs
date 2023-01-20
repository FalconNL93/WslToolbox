using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Models;

public partial class DownloadUpdateModel : ObservableRecipient
{
    [ObservableProperty]
    private int _current;

    [ObservableProperty]
    private string _currentHuman;

    [ObservableProperty]
    private bool _downloading;

    [ObservableProperty]
    private int _maximum;

    [ObservableProperty]
    private string _maximumHuman;
}