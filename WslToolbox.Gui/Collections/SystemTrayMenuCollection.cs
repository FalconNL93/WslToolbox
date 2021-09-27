using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class SystemTrayMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel) => new()
        {
            new MenuItem()
            {
                Header = "Show Application",
                Command = viewModel.ShowApplicationCommand
            },
            new MenuItem()
            {
                Header = "WSL Service",
                ItemsSource = new CompositeCollection()
                    {
                        new MenuItem()
                        {
                            Header = "Start WSL Service",
                            Command = viewModel.StartWslServiceCommand
                        },
                        new MenuItem()
                        {
                            Header = "Stop WSL Service",
                            Command = viewModel.StopWslServiceCommand
                        },
                        new MenuItem()
                        {
                            Header = "Restart WSL Service",
                            Command = viewModel.RestartWslServiceCommand
                        },
                    }
            },
            new MenuItem()
            {
                Header = "Settings",
                Command = viewModel.ShowSettingsCommand
            },
            new Separator(),
            new MenuItem()
            {
                Header = "Exit Application",
                Command = viewModel.ExitApplicationCommand
            },
        };
    }
}