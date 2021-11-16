using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class ManageMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Install distribution",
                    Command = viewModel.ShowSelectDialog
                },
                new MenuItem
                {
                    Header = "Import distribution",
                    Command = viewModel.ShowImportDialog
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Check for updates",
                    Command = viewModel.CheckForUpdates,
                    CommandParameter = true
                },
                new MenuItem
                {
                    Header = "Settings",
                    Command = viewModel.ShowSettings
                },
                new MenuItem
                {
                    Header = "Open application log",
                    Command = viewModel.OpenLogFile
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Exit application",
                    Command = viewModel.ExitApplication
                }
            };
        }
    }
}