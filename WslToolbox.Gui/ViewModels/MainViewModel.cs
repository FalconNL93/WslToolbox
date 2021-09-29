using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using WslToolbox.Core;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class MainViewModel
    {
        private readonly MainView View;

        public readonly ConfigurationHandler Config = new();
        public ICommand ShowApplicationCommand => new RelayCommand(ShowApplication, o => View.WindowState == System.Windows.WindowState.Minimized);
        public ICommand SaveConfigurationCommand => new RelayCommand(o => { Config.Save(); }, o => true);
        public ICommand ExitApplicationCommand => new RelayCommand(o => { Environment.Exit(-1); }, o => true);
        public ICommand StartWslServiceCommand => new RelayCommand(StartWslService, o => true);
        public ICommand StopWslServiceCommand => new RelayCommand(StopWslService, o => true);
        public ICommand RestartWslServiceCommand => new RelayCommand(RestartWslService, o => true);
        public ICommand ShowSettingsCommand => new RelayCommand(ShowSettings, o => true);
        public ICommand StartDistributionCommand => new RelayCommand(StartDistribution, o => true);
        public ICommand StopDistributionCommand => new RelayCommand(StopDistribution, o => true);
        public ICommand OpenLogFileCommand => new RelayCommand(OpenLogFile, o => File.Exists(LogConfiguration.FileName));

        public Timer ServicePoller;
        public DistributionClass SelectedDistribution { get; set; }

        public MainViewModel(MainView view)
        {
            View = view;

            InitializeEventHandlers();
            PollTimerInitializer();
        }

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

        public void ShowSettings(object parameter)
        {
            LogHandler.Log().Debug("Debug log");
            LogHandler.Log().Error("Error log");
            SettingsView settingsWindow = new(Config.Configuration, Config);
            settingsWindow.ShowDialog();

            if ((bool)settingsWindow.DialogResult && SaveConfigurationCommand.CanExecute(null))
            {
                SaveConfigurationCommand.Execute(null);
            }
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

        public async void StartDistribution(object parameter)
        {
            _ = await ToolboxClass.StartDistribution((DistributionClass)parameter);
        }

        public async void StopDistribution(object parameter)
        {
            _ = await ToolboxClass.TerminateDistribution((DistributionClass)parameter);
        }

        public void PollTimerInitializer(int interval = 2000)
        {
            ServicePoller = Config.Configuration.PollServiceStatus ? new(PollCallBack, null, 0, interval) : null;
        }

        private async void PollCallBack(object o)
        {
            bool isRunning = await ToolboxClass.ServiceIsRunning();
        }

        public void ShowApplication(object o)
        {
            View.WindowState = System.Windows.WindowState.Normal;
            View.Show();
        }

        public void SaveSuccessfullyEvent(object sender, EventArgs e)
        {
            View.HandleConfiguration();
        }

        public void OpenLogFile(object parameter)
        {
            _ = Process.Start(new ProcessStartInfo("explorer")
            {
                Arguments = Path.GetFullPath(LogConfiguration.FileName)
            });
        }
    }
}