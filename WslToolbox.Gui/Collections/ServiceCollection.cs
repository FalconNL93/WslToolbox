using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class ServiceCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new Button
                {
                    Content = "Start",
                    Command = viewModel.StartWslService
                },
                new Button
                {
                    Content = "Stop",
                    Command = viewModel.StopWslService
                },
                new Button
                {
                    Content = "Restart",
                    Command = viewModel.RestartWslService
                },
                new Button
                {
                    Content = "Refresh",
                    Command = viewModel.Refresh
                },
                new Button
                {
                    Content = "Update",
                    Command = viewModel.UpdateWslService
                }
            };
        }
    }
}