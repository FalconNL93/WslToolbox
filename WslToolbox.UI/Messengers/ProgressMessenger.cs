using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Core.Args;

namespace WslToolbox.UI.Messengers;

public class ProgressChangedMessage : ValueChangedMessage<UserProgressChangedEventArgs>
{
    public ProgressChangedMessage(UserProgressChangedEventArgs value) : base(value)
    {
    }
}