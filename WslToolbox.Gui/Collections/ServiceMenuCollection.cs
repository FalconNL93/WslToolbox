using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class ServiceMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Refresh",
                    Command = viewModel.Refresh
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Start",
                    Command = viewModel.StartWslService
                },
                new MenuItem
                {
                    Header = "Stop",
                    Command = viewModel.StopWslService
                },
                new MenuItem
                {
                    Header = "Restart",
                    Command = viewModel.RestartWslService
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Update WSL",
                    Command = viewModel.UpdateWslService
                }
            };
        }
    }
}