using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public partial class UserOptions : ObservableRecipient
{
    [ObservableProperty]
    private bool _notifications = true;

    [ObservableProperty]
    private string _theme = "Default";
    
    [ObservableProperty]
    private int _shellBehaviour;

    public bool SeenWelcomePage { get; set; }
    public bool HideDocker { get; set; } = true;
}