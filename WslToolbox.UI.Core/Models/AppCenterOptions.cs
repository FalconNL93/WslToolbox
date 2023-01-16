using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public enum AppCenterStates
{
    IsUnavailable,
    IsAvailable,
    IsEnabled
}

public partial class AppCenterOptions : ObservableRecipient
{
    [ObservableProperty]
    private bool _isAvailable;
}