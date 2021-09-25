using System;
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
        public ICommand RestartWslServiceCommand => new RelayCommand(RestartWslService, o => true);
        public ICommand ShowSettingsCommand => new RelayCommand(ShowSettings, o => true);

        public MainViewModel(MainView view)
        {
            View = view;
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
                new Separator(),
                new MenuItem()
                {
                    Header = "Restart WSL Service",
                    Command = RestartWslServiceCommand
                },
                new MenuItem()
                {
                    Header = "Settings",
                    Command = ShowSettingsCommand
                },
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
            bool settingsWindowDialogResult = (bool)settingsWindow.DialogResult;

            if (settingsWindowDialogResult)
            {
                if (SaveConfigurationCommand.CanExecute(null))
                {
                    SaveConfigurationCommand.Execute(null);
                }
            }
        }

        public async void RestartWslService(object parameter)
        {
            _ = await ToolboxClass.StopWsl().ConfigureAwait(true);
            _ = await ToolboxClass.StartWsl().ConfigureAwait(true);
        }
    }
}