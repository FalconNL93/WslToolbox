using CommunityToolkit.WinUI.Notifications;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Notifications;

public static class UpdateNotification
{
    public static void ShowNoUpdatesNotification()
    {
        new ToastContentBuilder()
            .AddText("No new updates available")
            .Show();
    }

    public static void ShowUpdatesAvailableNotification(UpdateResultModel updateResult)
    {
        new ToastContentBuilder()
            .AddText($"New update {updateResult.LatestVersion} available")
            .AddButton(new ToastButton()
                .SetContent("Download")
                .AddArgument("url", updateResult.DownloadUri.ToString()))
            .AddButton(new ToastButton()
                .SetContent("Skip"))
            .Show();
    }
}