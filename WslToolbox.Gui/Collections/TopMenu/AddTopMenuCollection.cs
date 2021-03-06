using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.TopMenu
{
    public static class AddTopMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Install distribution",
                    Command = viewModel.ShowInstallDistributionDialog
                },
                new MenuItem
                {
                    Header = "Import distribution",
                    Command = viewModel.ShowImportDialog
                }
            };
        }
    }
}