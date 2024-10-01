using CommunityToolkit.Mvvm.Messaging.Messages;
using H.NotifyIcon.Core;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Messengers;

public class UserOptionsChanged(UserOptions value) : RequestMessage<UserOptions>
{
    public UserOptions UserOptions { get; set; } = value;
}

public class ShowTrayIcon(TrayIcon value) : RequestMessage<TrayIcon>
{
    public TrayIcon TrayIcon { get; set; } = value;
}

public class HideTrayIcon : RequestMessage<bool>
{
}