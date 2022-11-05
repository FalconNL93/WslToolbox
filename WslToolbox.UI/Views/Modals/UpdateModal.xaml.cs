using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.ViewModels;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class UpdateModal : ModalPage
{
    public UpdateModal()
    {
        ViewModel = App.GetService<UpdateViewModel>();
        InitializeComponent();
    }

    public UpdateViewModel ViewModel { get; }
}