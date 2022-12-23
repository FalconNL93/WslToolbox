using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class DeveloperPage : Page
{
    public DeveloperPage()
    {
        ViewModel = App.GetService<DeveloperViewModel>();
        InitializeComponent();
    }

    public DeveloperViewModel ViewModel { get; }

    private void OnFakeUpdateSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = FakeUpdateResultsSelector.SelectedItem;
        ViewModel.DevOptions.Value.FakeUpdateResult = selectedItem switch
        {
            nameof(FakeUpdateResult.UpdateAvailable) => FakeUpdateResult.UpdateAvailable,
            nameof(FakeUpdateResult.NoUpdate) => FakeUpdateResult.NoUpdate,
            _ => FakeUpdateResult.Off,
        };
    }
}