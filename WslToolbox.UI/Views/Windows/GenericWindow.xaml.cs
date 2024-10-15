using Microsoft.UI;
using WslToolbox.UI.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WslToolbox.UI.Views.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GenericWindow : WindowEx
    {
        public GenericWindow()
        {
            InitializeComponent();

            this.SetTitlebarColor(Colors.White);
        }
    }
}