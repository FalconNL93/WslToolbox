using Microsoft.Toolkit.Uwp.Notifications;

namespace WslToolbox.UI.Notifications;

public static class UpdateNotification
{
    public static string Build()
    {
        var builder = new ToastContentBuilder();

        builder.AddText("No new updates available");

        return builder.GetXml().GetXml();
    }
}