using System.Windows.Controls;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Forms;

public partial class EditDistributionForm : UserControl
{
    public EditDistributionForm()
    {
        InitializeComponent();
    }

    public DistributionModel Distribution { get; set; } = new();
}