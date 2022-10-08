using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WslToolbox.UI.Services;

public class ProgressModel
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsIndeterminate { get; set; } = true;
    public bool ShowPaused { get; set; }
    public bool ShowError { get; set; }
    public bool ShowProgress { get; set; }
    public bool IsPrimaryButtonEnabled { get; set; }
    public bool IsSecondaryButtonEnabled { get; set; }
}

public class ProgressIndicatorChangedMessage : ValueChangedMessage<ProgressModel>
{
    public ProgressIndicatorChangedMessage(ProgressModel value) : base(value)
    {
    }
}

public class ProgressIndicatorRequestMessage : RequestMessage<ProgressModel>
{
}