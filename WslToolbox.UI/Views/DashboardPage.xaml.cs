using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Messengers;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

        ViewModel.RefreshDistributions.Execute(null);
    }

    public DashboardViewModel ViewModel { get; }
}