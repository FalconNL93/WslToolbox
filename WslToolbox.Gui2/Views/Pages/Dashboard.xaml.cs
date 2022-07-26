using Wpf.Ui.Common.Interfaces;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public partial class Dashboard : INavigableView<DashboardViewModel>
{
    public Dashboard(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public DashboardViewModel ViewModel { get; }
}