using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class DataGridMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new()
            {
                new MenuItem
                {
                    Header = viewModel.SelectedDistribution.Name,
                    IsEnabled = false
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Shell"
                },
                new Separator(),
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
                    Header = "Set default"
                },
                new MenuItem
                {
                    Header = "Convert"
                }
            };
        }
    }
}