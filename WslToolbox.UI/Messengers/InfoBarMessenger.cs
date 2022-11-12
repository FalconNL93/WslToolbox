using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Models;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.Messengers;

public class InfoBarChangedMessage : ValueChangedMessage<InfoBarModel>
{
    public InfoBarChangedMessage(InfoBarModel value) : base(value)
    {
    }
}

public class InfoBarRequestMessage : RequestMessage<ProgressModel>
{
}