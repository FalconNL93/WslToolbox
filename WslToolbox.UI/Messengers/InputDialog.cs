using CommunityToolkit.Mvvm.Messaging.Messages;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.Messengers;

public class InputDialogModel
{
    public string Message { get; set; }
    public string Title { get; set; } = App.Name;
    public string InputFieldText { get; set; }
}

public class InputDialogMessage : RequestMessage<InputDialog>
{
    public InputDialogMessage(InputDialogModel value)
    {
        ViewModel = value;
    }

    public InputDialogModel ViewModel { get; set; }
}