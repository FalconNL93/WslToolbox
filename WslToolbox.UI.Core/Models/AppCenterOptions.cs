using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public partial class AppCenterOptions : ObservableRecipient
{
    [ObservableProperty]
    private bool _isAvailable;
}