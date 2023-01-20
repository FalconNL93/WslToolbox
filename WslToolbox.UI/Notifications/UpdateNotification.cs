using System.Reflection;
using CommunityToolkit.WinUI.Notifications;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.Notifications;

public static class UpdateNotification
{
    public static ToastContentBuilder NoUpdatesNotification =>
        new ToastContentBuilder()
            .AddHeader(MethodBase.GetCurrentMethod().Name, "Updates", new ToastArguments())
            .AddText("You are running the latest version");


    public static ToastContentBuilder UpdatesAvailable(UpdateResultModel updateResult) =>
        new ToastContentBuilder()
            .AddHeader(MethodBase.GetCurrentMethod().Name, "Updates", new ToastArguments())
            .AddText($"New update {updateResult.LatestVersion} available")
            .AddButton(new ToastButton()
                .SetContent("Download")
                .AddArgument(ToastActions.DownloadUpdate))
            .AddButton(new ToastButton()
                .SetContent("Close"));
}