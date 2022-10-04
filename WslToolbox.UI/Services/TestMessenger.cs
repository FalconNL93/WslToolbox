using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WslToolbox.UI.Services;

public class ProgressIndicator
{
    public string Title { get; set; }
    public string Message { get; set; }
}

public class ProgressIndicatorChangedMessage : ValueChangedMessage<ProgressIndicator>
{
    public ProgressIndicatorChangedMessage(ProgressIndicator value) : base(value)
    {
    }
}

public class ProgressIndicatorRequestMessage : RequestMessage<ProgressIndicator>
{
}
