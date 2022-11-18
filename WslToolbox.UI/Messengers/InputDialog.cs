using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Messengers;

public class InputDialogModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string DefaultInput { get; set; }
}

public class ShowInputDialogChangedMessage : ValueChangedMessage<InputDialogModel>
{
    public ShowInputDialogChangedMessage(InputDialogModel value) : base(value)
    {
    }
}

public class InputDialogRequestMessage : AsyncRequestMessage<ContentDialogResult>
{
    public string DefaultValue;
    public string Message;
    public string Title;

    public InputDialogRequestMessage(string title, string message, string defaultValue = "")
    {
        Title = title;
        Message = message;
        DefaultValue = defaultValue;
    }
}