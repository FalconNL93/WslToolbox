using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.TopMenu
{
    public static class MoreTopMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Settings",
                    Command = viewModel.ShowSettings
                },
                new MenuItem
                {
                    Header = "Actions",
                    ItemsSource = ActionItems(viewModel)
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

        private static CompositeCollection ActionItems(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Enable required Windows components",
                    Command = viewModel.EnableWindowsComponents
                }
            };
        }
    }
}