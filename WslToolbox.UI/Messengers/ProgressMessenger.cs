using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.Messengers;

public class ProgressChangedMessage : ValueChangedMessage<UserProgressChangedEventArgs>
{
    public ProgressChangedMessage(UserProgressChangedEventArgs value) : base(value)
    {
    }
}