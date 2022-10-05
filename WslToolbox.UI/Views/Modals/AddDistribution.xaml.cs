using WslToolbox.UI.Contracts.Views;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Views.Modals;

public sealed partial class AddDistribution : ModalPage
{
    private readonly List<Distribution> _distributions;

    public AddDistribution(List<Distribution> distributions)
    {
        _distributions = distributions;
        InitializeComponent();
    }
}