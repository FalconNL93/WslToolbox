using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WslToolbox.UI.Services;

public class UpdateMessenger
{
    public bool Silent { get; set; }
}

public class UpdateChangedMessage : ValueChangedMessage<UpdateMessenger>
{
    public UpdateChangedMessage(UpdateMessenger value) : base(value)
    {
    }
}

public class UpdateChangedRequestMessage : RequestMessage<UpdateMessenger>
{
}