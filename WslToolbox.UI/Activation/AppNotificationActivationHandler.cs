using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using WslToolbox.UI.Contracts.Services;

namespace WslToolbox.UI.Activation;

public class AppNotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;
    private readonly IAppNotificationService _notificationService;

    public AppNotificationActivationHandler(INavigationService navigationService, IAppNotificationService notificationService)
    {
        _navigationService = navigationService;
        _notificationService = notificationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        return AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
        {
            App.MainWindow.ShowMessageDialogAsync("TODO: Handle notification activations.", "Notification Activation");
        });

        await Task.CompletedTask;
    }
}