using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Messengers;

public class UpdateInfoBarChangedMessage : ValueChangedMessage<InfoBarModel>
{
    public UpdateInfoBarChangedMessage(InfoBarModel value) : base(value)
    {
    }
}

public class UpdateInfoBarRequestMessage : RequestMessage<InfoBarModel>
{
}