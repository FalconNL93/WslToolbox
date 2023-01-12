using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Messengers;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class ImportDistribution : ContentDialog
{
    public ImportDistribution(ImportDialogViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public async Task<ImportDialogViewModel> ShowInput()
    {
        ViewModel.ContentDialogResult = await ShowAsync();
        return ViewModel;
    }

    public ImportDialogViewModel ViewModel { get; set; }
}