using System.Collections.Specialized;
using System.Web;
using Microsoft.Windows.AppNotifications;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Services;

public static class NotificationActions
{
    public const string OpenUrl = nameof(OpenUrl);
    public const string Show = nameof(Show);
}

public class AppNotificationService : IAppNotificationService
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

        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
        AppNotificationManager.Default.Register();
    }

    public bool Show(string payload)
    {
        var appNotification = new AppNotification(payload);

        AppNotificationManager.Default.Show(appNotification);

        return appNotification.Id != 0;
    }

    public NameValueCollection ParseArguments(string arguments)
    {
        return HttpUtility.ParseQueryString(arguments);
    }

    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }

    ~AppNotificationService()
    {
        Unregister();
    }

    private static void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        foreach (var arg in args.Arguments)
        {
            switch (arg.Key)
            {
                case NotificationActions.OpenUrl:
                    ShellHelper.OpenUrl(new Uri(arg.Value));
                    break;
                case NotificationActions.Show:
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        App.MainWindow.BringToFront();
                    });
                    break;
            }
        }
    }
}