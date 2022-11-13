using WslToolbox.UI.Contracts.Views;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class ErrorModal : ModalPage
{
    private readonly string _message;

    public ErrorModal(object parameter)
    {
        _message = (string) parameter;

        InitializeComponent();
    }
}