using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using WslToolbox.Core;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Exceptions;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class MainViewModel
    {
        public readonly ConfigurationHandler Config = new();
        private readonly MainView _view;

        private Timer _servicePoller;

        public MainViewModel(MainView view)
        {
            _view = view;

            InitializeEventHandlers();
            PollTimerInitializer();
        }

        public ICommand ShowApplicationCommand =>
            new RelayCommand(ShowApplication, o => _view.WindowState == WindowState.Minimized);

        public ICommand SaveConfigurationCommand => new RelayCommand(SaveConfiguration, o => true);
        public ICommand ExitApplicationCommand => new RelayCommand(o => { Environment.Exit(-1); }, o => true);
        public ICommand StartWslServiceCommand => new RelayCommand(StartWslService, o => CanStartWslService);
        public ICommand StopWslServiceCommand => new RelayCommand(StopWslService, o => true);
        public ICommand RestartWslServiceCommand => new RelayCommand(RestartWslService, o => true);
        public ICommand ShowSettingsCommand => new RelayCommand(ShowSettings, o => true);
        public ICommand StartDistributionCommand => new RelayCommand(StartDistribution, o => true);
        public ICommand StopDistributionCommand => new RelayCommand(StopDistribution, o => true);
        
        public ICommand StartOnBoot => new RelayCommand(null, o => false);

        public ICommand OpenLogFileCommand =>
            new RelayCommand(OpenLogFile, o => File.Exists(LogConfiguration.FileName));

        public DistributionClass SelectedDistribution { get; set; }

        private bool CanStartWslService { get; set; } = true;

        private void InitializeEventHandlers()
        {
            Config.ConfigurationUpdatedSuccessfully += SaveSuccessfullyEvent;
        }

        public CompositeCollection SystemTrayMenuItems()
        {
            return SystemTrayMenuCollection.Items(this);
        }

        public CompositeCollection DataGridMenuItems()
        {
            return DataGridMenuCollection.Items(this);
        }

        private void ShowSettings(object parameter)
        {
            SettingsView settingsWindow = new(Config.Configuration, Config);
            settingsWindow.ShowDialog();

            if (settingsWindow.DialogResult != null && (bool) settingsWindow.DialogResult &&
                SaveConfigurationCommand.CanExecute(null)) SaveConfigurationCommand.Execute(null);
        }

        private async void StartWslService(object parameter)
        {
            CanStartWslService = false;
            _ = await ToolboxClass.StartWsl().ConfigureAwait(true);
            CanStartWslService = true;
        }

        private async void StopWslService(object parameter)
        {
            CanStartWslService = false;
            _ = await ToolboxClass.StopWsl().ConfigureAwait(true);
            CanStartWslService = true;
        }

        private void RestartWslService(object parameter)
        {
            StopWslServiceCommand.Execute(null);
            StartWslServiceCommand.Execute(null);
        }

        private async void StartDistribution(object parameter)
        {
            _ = await ToolboxClass.StartDistribution((DistributionClass) parameter);
        }

        private async void StopDistribution(object parameter)
        {
            _ = await ToolboxClass.TerminateDistribution((DistributionClass) parameter);
        }

        private void PollTimerInitializer(int interval = 2000)
        {
            _servicePoller = Config.Configuration.PollServiceStatus ? new Timer(PollCallBack, null, 0, interval) : null;
        }

        private async void PollCallBack(object o)
        {
            await ToolboxClass.ServiceIsRunning();
        }

        private void ShowApplication(object o)
        {
            _view.WindowState = WindowState.Normal;
            _view.Show();
        }

        private void SaveSuccessfullyEvent(object sender, EventArgs e)
        {
            LogHandler.Log().Debug("Configuration file saved, reloading configuration");
            _view.HandleConfiguration();
        }

        private async void SaveConfiguration(object parameter)
        {
            try
            {
                Config.Save();
            }
            catch (ConfigurationFileNotSavedException e)
            {
                LogHandler.Log().Error(e.Message, e);
                await _view.ShowMessageAsync("Error", "Your configuration is not saved due to an error.");
            }
        }

        private void OpenLogFile(object parameter)
        {
            _ = Process.Start(new ProcessStartInfo("explorer")
            {
                Arguments = Path.GetFullPath(LogConfiguration.FileName)
            });
        }
    }
}