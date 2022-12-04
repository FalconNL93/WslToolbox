using CommunityToolkit.WinUI.Notifications;
using WslToolbox.UI.Contracts.Services;

namespace WslToolbox.UI.Services;

public class AppNotificationService
{
    private readonly INavigationService _navigationService;

    public AppNotificationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void Initialize()
    {
        if (App.IsPackage())
        {
            return;
        }

        ToastNotificationManagerCompat.OnActivated += args =>
        {
        };
    }
}