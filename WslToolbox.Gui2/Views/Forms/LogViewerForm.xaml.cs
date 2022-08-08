using System.Windows.Controls;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Views.Forms;

public partial class LogViewerForm : UserControl
{
    public LogViewerForm()
    {
        InitializeComponent();
    }

    public DistributionModel Distribution { get; set; } = new();
    public string? Log { get; set; }
    public bool ReadOnly { get; set; }
}