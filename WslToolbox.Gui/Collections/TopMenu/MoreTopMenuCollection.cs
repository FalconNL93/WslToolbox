using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Commands.Distribution;
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
                new MenuItem
                {
                    Header = "Open application directory",
                    Command = viewModel.OpenAppFolder
                },
                new Separator(),
                new MenuItem
                {
                    Header = "About",
                    Command = viewModel.ShowAboutDialog
                },
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
                    Header = "Clear online distribution list cache",
                    Command = viewModel.ShowInstallDistributionDialog,
                    CommandParameter = InstallDistributionCommand.Parameters.ClearCache
                },
                new MenuItem
                {
                    Header = "Open WSL app in Windows Store",
                    Command = new OpenUrlCommand(),
                    CommandParameter = "https://aka.ms/wslstorepage"
                },
                new MenuItem
                {
                    Header = "Enable required Windows components",
                    Command = viewModel.EnableWindowsComponents
                }
            };
        }
    }
}