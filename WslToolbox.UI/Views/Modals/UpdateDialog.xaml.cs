using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class UpdateDialog : ContentDialog
{
    public UpdateDialog(UpdateViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }

    public UpdateViewModel ViewModel { get; }
}