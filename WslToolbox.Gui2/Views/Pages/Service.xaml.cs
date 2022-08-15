using Wpf.Ui.Common.Interfaces;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public partial class Service : INavigableView<ServiceViewModel>
{
    public Service(ServiceViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public ServiceViewModel ViewModel { get; }
}