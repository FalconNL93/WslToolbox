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
                    IsEnabled = false
                },
                new MenuItem
                {
                    Header = viewModel.SelectedDistribution.Guid,
                    IsEnabled = false
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Open base path",
                    Command = viewModel.OpenBasePathDistributionCommand,
                    CommandParameter = viewModel.SelectedDistribution,
                    ToolTip = viewModel.SelectedDistribution.BasePathLocal
                },
                new MenuItem
                {
                    Header = "Rename...",
                    Command = viewModel.RenameDistributionCommand,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Change base path...",
                    Command = viewModel.ChangeBasePathDistributionCommand,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Open shell"
                },
                new MenuItem
                {
                    Header = "Start",
                    Command = viewModel.StartDistributionCommand,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Stop",
                    Command = viewModel.StopDistributionCommand,
                    CommandParameter = viewModel.SelectedDistribution
                },
                new MenuItem
                {
                    Header = "Restart"
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Set as default distribution"
                },
                new MenuItem
                {
                    Header = "Convert to WSL2"
                }
            };
        }
    }
}