using CommunityToolkit.WinUI.Notifications;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Services;

public static class ToastActions
{
    public const string OpenUrl = nameof(OpenUrl);
    public const string ShowWindow = nameof(ShowWindow);
}

public class AppNotificationService
{
    private readonly INavigationService _navigationService;

    public AppNotificationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void Initialize()
    {
        ToastNotificationManagerCompat.OnActivated += ToastOnActivated;
    }

    private static void ToastOnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        var arguments = ToastArguments.Parse(e.Argument);
        if (arguments == null || !arguments.Any())
        {
            return;
        }

        var userInput = e.UserInput;
        foreach (var argument in arguments)
        {
            switch (argument.Key)
            {
                case ToastActions.OpenUrl:
                    ShellHelper.OpenUrl(new Uri(argument.Value));
                    break;
                case ToastActions.ShowWindow:
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        App.MainWindow.BringToFront();
                    });
                    break;
            }
        }
    }
}