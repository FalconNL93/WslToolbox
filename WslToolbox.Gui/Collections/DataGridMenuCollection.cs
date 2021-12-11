using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class DataGridMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Terminal",
                    Command = viewModel.OpenDistributionShell,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Start",
                    Command = viewModel.StartDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Stop",
                    Command = viewModel.StopDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Restart",
                    Command = viewModel.RestartDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Rename",
                    Command = viewModel.RenameDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Export",
                    Command = viewModel.ShowExportDialog,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Delete",
                    Command = viewModel.DeleteDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new Separator(),
                new MenuItem
                {
                    Header = "More",
                    ItemsSource = MoreMenuItems(viewModel)
                }
            };
        }

        private static CompositeCollection MoreMenuItems(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Set as default",
                    Command = viewModel.SetDefaultDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Convert to WSL2",
                    Visibility = viewModel.SelectedDistribution.Version == 2 ? Visibility.Collapsed : Visibility.Visible
                },
                new MenuItem
                {
                    Header = "Open base path",
                    Command = viewModel.OpenBasePathDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "GUID",
                    ItemsSource = new CompositeCollection
                    {
                        new MenuItem
                        {
                            Header = "Copy to clipboard",
                            Command = new CopyToClipboardCommand(),
                            CommandParameter = viewModel.SelectedDistribution.Guid
                        },
                        new Separator(),
                        new MenuItem {Header = viewModel.SelectedDistribution.Guid}
                    }
                }
            };
        }
    }
}