using Wpf.Ui.Common.Interfaces;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public class Dashboard : INavigableView<DashboardViewModel>
{
    public DashboardViewModel ViewModel { get; }

    public Dashboard(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}