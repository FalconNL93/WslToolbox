using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class MoveDialog : ContentDialog
{
    public MoveDialog(MoveDialogViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public MoveDialogViewModel ViewModel { get; set; }

    public async Task<MoveDialogViewModel> ShowInput()
    {
        ViewModel.ContentDialogResult = await ShowAsync();
        return ViewModel;
    }
}