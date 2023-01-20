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

    public async Task<InputDialogModel> ShowInput()
    {
        ViewModel.ContentDialogResult = await ShowAsync();
        return ViewModel;
    }
}