using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public partial class UserOptions : ObservableRecipient
{
    [ObservableProperty]
    private bool _notifications = true;

    public bool SeenWelcomePage { get; set; }
    public bool HideDocker { get; set; } = true;
    public string Theme { get; set; } = "Default";
}