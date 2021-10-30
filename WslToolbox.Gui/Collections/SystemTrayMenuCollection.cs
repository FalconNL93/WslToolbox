using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class SystemTrayMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new MenuItem
                {
                    Header = "Show Application",
                    Command = viewModel.ShowApplication
                },
                new MenuItem
                {
                    Header = "WSL Service",
                    ItemsSource = new CompositeCollection
                    {
                        new MenuItem
                        {
                            Header = "Start WSL Service",
                            Command = viewModel.StartWslService
                        },
                        new MenuItem
                        {
                            Header = "Stop WSL Service",
                            Command = viewModel.StopWslService
                        },
                        new MenuItem
                        {
                            Header = "Restart WSL Service",
                            Command = viewModel.RestartWslService
                        }
                    }
                },
                new MenuItem
                {
                    Header = "Settings",
                    Command = viewModel.ShowSettings
                },
                new Separator(),
                new MenuItem
                {
                    Header = "Exit Application",
                    Command = viewModel.ExitApplication,
                    CommandParameter = 1
                }
            };
        }
    }
}