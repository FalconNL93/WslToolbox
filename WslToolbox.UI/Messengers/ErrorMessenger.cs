using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Messengers;

public class ErrorChangedMessage : ValueChangedMessage<ErrorModel>
{
    public ErrorChangedMessage(ErrorModel value) : base(value)
    {
    }
}

public class ErrorRequestMessage : RequestMessage<ErrorModel>
{
}