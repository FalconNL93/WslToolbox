using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class UpdatingDialog : ContentDialog
{
    public UpdatingDialog(UpdatingDialogViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public UpdatingDialogViewModel ViewModel { get; set; }

    private void UpdatingDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        args.Cancel = true;
    }
}