using Wpf.Ui.Common.Interfaces;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public partial class Information : INavigableView<InformationViewModel>
{
    public Information(InformationViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public InformationViewModel ViewModel { get; }
}