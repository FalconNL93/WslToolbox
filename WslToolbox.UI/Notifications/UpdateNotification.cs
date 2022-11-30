using Microsoft.Toolkit.Uwp.Notifications;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.Notifications;

public static class UpdateNotification
{
    public static void ShowNoUpdatesNotification(this IAppNotificationService notificationService)
    {
        notificationService.Show(new ToastContentBuilder()
            .AddText("No new updates available")
            .GetXml()
            .GetXml());
    }

    public static void ShowUpdatesAvailableNotification(this IAppNotificationService notificationService, UpdateResultModel updateResult)
    {
        notificationService.Show(new ToastContentBuilder()
            .AddText($"New update {updateResult.LatestVersion} available")
            .AddButton(new ToastButton()
                .SetContent("Download")
                .AddArgument(NotificationActions.OpenUrl, updateResult.DownloadUri.ToString()))
            .AddButton(new ToastButton()
                .SetContent("Skip"))
            .GetXml()
            .GetXml());
    }
}