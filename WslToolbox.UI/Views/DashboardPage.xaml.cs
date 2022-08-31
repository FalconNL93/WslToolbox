using Microsoft.UI.Xaml.Controls;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
    }

    public DashboardViewModel ViewModel { get; }
}