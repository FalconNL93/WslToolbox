using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Pages;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        ViewModel.RefreshDistributionsCommand.Execute(null);
    }

    public DashboardViewModel ViewModel { get; }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        ViewModel.ShowStartupDialogCommand.Execute(null);
    }
}