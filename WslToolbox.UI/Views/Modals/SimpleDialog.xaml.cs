using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Models;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class SimpleDialog : ContentDialog
{
    public SimpleDialog(SimpleDialogModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public SimpleDialogModel ViewModel { get; }
}