using System.Windows.Controls;
using System.Windows.Data;
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
                    Header = viewModel.SelectedDistribution.Name,
                    ItemsSource = new CompositeCollection
                    {
                        new MenuItem
                        {
                            Header = "Rename",
                            Command = viewModel.RenameDistribution,
                            CommandParameter = viewModel.SelectedDistribution
                        }
                    }
                },
                new MenuItem
                {
                    Header = "GUID",
                    ItemsSource = new CompositeCollection
                    {
                        new MenuItem
                        {
                            Header = viewModel.SelectedDistribution.Guid,
                            IsEnabled = false
                        },
                        new MenuItem
                        {
                            Header = "Copy GUID to clipboard",
                            Command = viewModel.CopyToClipboard,
                            CommandParameter = viewModel.SelectedDistribution.Guid
                        }
                    }
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Base path",
                    ItemsSource = new CompositeCollection
                    {
                        new MenuItem
                        {
                            Header = "Copy base path to clipboard",
                            Command = viewModel.CopyToClipboard,
                            CommandParameter = viewModel.SelectedDistribution.BasePathLocal,
                            ToolTip = viewModel.SelectedDistribution.BasePathLocal
                        },
                        new MenuItem
                        {
                            Header = "Open base path",
                            Command = viewModel.OpenBasePathDistribution,
                            CommandParameter = viewModel.SelectedDistribution,
                            ToolTip = viewModel.SelectedDistribution.BasePathLocal
                        },
                        new MenuItem
                        {
                            Header = "Change base path...",
                            IsEnabled = false
                        }
                    }
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Open shell",
                    Command = viewModel.OpenDistributionShell,
                    CommandParameter = viewModel.SelectedDistribution
                },
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
                    Command = viewModel.RestartWslService,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Export",
                    Command = viewModel.ShowExportDialog,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Set as default distribution",
                    Command = viewModel.SetDefaultDistribution,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Convert to WSL2",
                    IsEnabled = false
                }
            };
        }
    }
}