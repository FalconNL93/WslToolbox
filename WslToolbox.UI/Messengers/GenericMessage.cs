using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Messengers;

public class UserOptionsChanged(UserOptions value) : RequestMessage<UserOptions>
{
    public UserOptions UserOptions { get; set; } = value;
}

public class BoolMessageReceived(bool value) : ValueChangedMessage<bool>(value);