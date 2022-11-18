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

public class InputDialogRequestMessage : RequestMessage<Task<ContentDialogResult>>
{
    public InputDialogModel InputDialogModel { get; set; }
    
    public InputDialogRequestMessage(InputDialogModel value)
    {
        InputDialogModel = value;
    }
}