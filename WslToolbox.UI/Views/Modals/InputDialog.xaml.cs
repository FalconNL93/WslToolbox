using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class InputDialog : ContentDialog
{
    public InputDialog(InputDialogModel viewModel)
    {
        ViewModel = viewModel;
        
        InitializeComponent();
    }

    public InputDialogModel ViewModel { get; set; }
}