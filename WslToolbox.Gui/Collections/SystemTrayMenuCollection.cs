using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Commands.Distribution;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections
{
    public static class SystemTrayMenuCollection
    {
        public static CompositeCollection Items(MainViewModel viewModel)
        {
            var systemTrayMenuItems = new CompositeCollection
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
                }
            };

            if (viewModel.Config.Configuration.ShowDistributionsInSystemTray)
            {
                systemTrayMenuItems.Add(new Separator());

                foreach (var distributionItem in DistributionCollection(viewModel))
                    systemTrayMenuItems.Add(distributionItem);
            }

            foreach (var bottomMenuItem in BottomMenuItems(viewModel)) systemTrayMenuItems.Add(bottomMenuItem);

            return systemTrayMenuItems;
        }

        private static CompositeCollection BottomMenuItems(MainViewModel viewModel)
        {
            return new CompositeCollection
            {
                new Separator(),
                new MenuItem
                {
                    Header = "Settings",
                    Command = viewModel.ShowSettings
                },
                new MenuItem
                {
                    Header = "Exit Application",
                    Command = viewModel.ExitApplication,
                    CommandParameter = 1
                }
            };
        }

        private static List<MenuItem> DistributionCollection(MainViewModel viewModel)
        {
            return viewModel.DistributionList.Select(distribution => new MenuItem
            {
                Header = distribution.Name,
                ItemsSource = new CompositeCollection
                {
                    new MenuItem
                    {
                        Header = "Start",
                        Command = new StartDistributionCommand(distribution)
                    },
                    new MenuItem
                    {
                        Header = "Stop",
                        Command = new StopDistributionCommand(distribution)
                    },
                    new MenuItem
                    {
                        Header = "Restart",
                        Command = new RestartDistributionCommand(distribution)
                    }
                }
            }).ToList();
        }
    }
}