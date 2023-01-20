using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class StartupDialog : ContentDialog
{
    public StartupDialog(StartupDialogViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public StartupDialogViewModel ViewModel { get; }
}