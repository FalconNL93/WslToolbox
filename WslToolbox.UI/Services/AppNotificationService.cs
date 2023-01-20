using CommunityToolkit.WinUI.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Services;

public static class ToastActions
{
    public const string OpenUrl = nameof(OpenUrl);
    public const string ShowWindow = nameof(ShowWindow);
    public const string DownloadUpdate = nameof(DownloadUpdate);
}

public class AppNotificationService
{
    private readonly ILogger<AppNotificationService> _logger;
    private readonly INavigationService _navigationService;
    private readonly UserOptions _userOptions;

    public AppNotificationService(INavigationService navigationService, IOptions<UserOptions> userOptions, ILogger<AppNotificationService> logger)
    {
        _navigationService = navigationService;
        _logger = logger;
        _userOptions = userOptions.Value;
    }

    public void Initialize()
    {
        if (!_userOptions.Notifications)
        {
            _logger.LogInformation("Notification system disabled");
            return;
        }

        _logger.LogInformation("Notification system enabled");
        ToastNotificationManagerCompat.OnActivated += ToastOnActivated;
    }

    public void Show(ToastContentBuilder notification)
    {
        if (!_userOptions.Notifications)
        {
            return;
        }

        notification.Show();
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
                case ToastActions.DownloadUpdate:
                    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                    {
                        App.MainWindow.BringToFront();
                    });
                    SettingsViewModel.OnDownloadUpdateEvent();
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