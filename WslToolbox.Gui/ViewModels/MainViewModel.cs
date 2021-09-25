using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WslToolbox.Core;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class MainViewModel
    {
        public readonly ConfigurationHandler Config = new();
        private readonly MainView View;
        public ICommand ShowApplicationCommand => new RelayCommand(o => { View.WindowState = System.Windows.WindowState.Normal; }, o => View.WindowState == System.Windows.WindowState.Minimized);
        public ICommand SaveConfigurationCommand => new RelayCommand(o => { Config.Save(); }, o => true);
        public ICommand ExitApplicationCommand => new RelayCommand(o => { Environment.Exit(-1); }, o => true);
        public ICommand StartWslServiceCommand => new RelayCommand(StartWslService, CanExecuteStart);
        public ICommand StopWslServiceCommand => new RelayCommand(StopWslService, CanExecuteStop);
        public ICommand RestartWslServiceCommand => new RelayCommand(RestartWslService, CanExecuteStop);
        public ICommand ShowSettingsCommand => new RelayCommand(ShowSettings, o => true);
        public Func<object, bool> CanExecuteStart = o => false;
        public Func<object, bool> CanExecuteStop = o => false;
        private Timer ServicePoller;

        public MainViewModel(MainView view)
        {
            View = view;

            PollTimerInitializer();
        }

        public CompositeCollection ContextMenuSystemTrayItems()
        {
            CompositeCollection contextMenuSystemTrayItems = new()
            {
                new MenuItem()
                {
                    Header = "Show Application",
                    Command = ShowApplicationCommand
                },
                new MenuItem()
                {
                    Header = "WSL Service",
                    ItemsSource = new CompositeCollection()
                    {
                        new MenuItem()
                        {
                            Header = "Start WSL Service",
                            Command = StartWslServiceCommand
                        },
                        new MenuItem()
                        {
                            Header = "Stop WSL Service",
                            Command = StopWslServiceCommand
                        },
                        new MenuItem()
                        {
                            Header = "Restart WSL Service",
                            Command = RestartWslServiceCommand
                        },
                    }
                },
                new MenuItem()
                {
                    Header = "Settings",
                    Command = ShowSettingsCommand
                },
                new Separator(),
                new MenuItem()
                {
                    Header = "Exit Application",
                    Command = ExitApplicationCommand
                },
            };

            return contextMenuSystemTrayItems;
        }

        public void ShowSettings(object parameter)
        {
            SettingsView settingsWindow = new(Config.Configuration, Config);
            settingsWindow.ShowDialog();
        }

        public async void StartWslService(object parameter)
        {
            _ = await ToolboxClass.StartWsl().ConfigureAwait(true);
        }

        public async void StopWslService(object parameter)
        {
            _ = await ToolboxClass.StopWsl().ConfigureAwait(true);
        }

        public void RestartWslService(object parameter)
        {
            StopWslServiceCommand.Execute(null);
            StartWslServiceCommand.Execute(null);
        }

        public async void CanStart(object parameter)
        {
            bool isRunning = await ToolboxClass.ServiceIsRunning();

            CanExecuteStart = o => !isRunning;
        }

        public async void CanStop(object parameter)
        {
            bool isRunning = await ToolboxClass.ServiceIsRunning();

            CanExecuteStop = o => isRunning;
        }

        public void PollTimerInitializer()
        {
            ServicePoller = new(PollCallBack, null, 0, 2000);
        }

        private async void PollCallBack(Object o)
        {
            bool isRunning = await ToolboxClass.ServiceIsRunning();
            CanExecuteStop = o => isRunning;
            CanExecuteStart = o => !isRunning;
        }
    }
}