using Microsoft.Toolkit.Uwp.Notifications;

namespace WslToolbox.UI.Notifications;

public static class UpdateNotification
{
    public static string NoUpdates =>
        new ToastContentBuilder()
            .AddText("No new updates available")
            .GetXml()
            .GetXml();
}