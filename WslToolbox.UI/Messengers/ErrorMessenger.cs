using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Models;
using WslToolbox.UI.Services;

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