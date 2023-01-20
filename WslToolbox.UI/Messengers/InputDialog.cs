using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.UI.Xaml.Controls;

namespace WslToolbox.UI.Messengers;

public class InputDialogModel
{
    public ContentDialogResult ContentDialogResult;
    public string PrimaryButtonText { get; set; } = "OK";
    public string SecondaryButtonText { get; set; }
    public string Message { get; set; }
    public string Title { get; set; } = App.Name;
    public string Result { get; set; }
}

public class InputDialogMessage : AsyncRequestMessage<InputDialogModel>
{
    public InputDialogMessage(InputDialogModel value)
    {
        ViewModel = value;
    }

    public InputDialogModel ViewModel { get; set; }
}