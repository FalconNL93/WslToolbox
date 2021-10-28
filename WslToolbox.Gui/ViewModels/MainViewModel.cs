using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CommandLine;
using MahApps.Metro.Controls.Dialogs;
using Serilog.Core;
using Serilog.Events;
using WslToolbox.Core;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Commands.Distribution;
using WslToolbox.Gui.Commands.Service;
using WslToolbox.Gui.Commands.Settings;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Properties;
using WslToolbox.Gui.Views;
using static WslToolbox.Gui.Handlers.LogHandler;

namespace WslToolbox.Gui.ViewModels
{
    public class Options
    {
        [Option('d', "debug", Default = false, HelpText = "Enable debug mode")]
        public bool DebugMode { get; set; }
    }

    public class MainViewModel
    {
        private readonly MainView _view;
        public readonly ConfigurationHandler Config = new();
        public readonly Logger Log;
        public readonly OsHandler OsHandler = new();
        public List<DistributionClass> DistributionList = new();

        public MainViewModel(MainView view)
        {
            var args = Environment.GetCommandLineArgs();
            Log = Log();
            _view = view;
            InitializeEventHandlers();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.DebugMode) DebugMode();
                });

            RefreshDistributions();
        }

        public ICommand ShowApplicationCommand =>
            new RelayCommand(ShowApplication, o => _view.WindowState == WindowState.Minimized);

        public ICommand NotImplementedCommand => new RelayCommand(o => { }, o => false);
        public ICommand SaveConfigurationCommand => new SaveSettingsCommand(Config);
        public ICommand ExitApplicationCommand => new RelayCommand(o => { Environment.Exit(-1); }, o => true);
        public ICommand RefreshCommand => new RelayCommand(RefreshDistributionsView, o => true);
        public ICommand StartDistributionCommand => new RelayCommand(StartDistribution, o => CanStartDistribution);
        public ICommand StopDistributionCommand => new RelayCommand(StopDistribution, o => CanStopDistribution);
        public ICommand RenameDistributionCommand => new RelayCommand(RenameDistribution, o => CanRenameDistribution);

        public ICommand SetDefaultDistributionCommand =>
            new RelayCommand(SetDefaultDistribution, CanSetDefaultDistribution);

        public ICommand ChangeBasePathDistributionCommand =>
            new RelayCommand(ChangeBasePathDistribution, o => CanChangeBasePathDistribution);

        public ICommand OpenBasePathDistributionCommand =>
            new RelayCommand(OpenBasePathDistribution, CanOpenBasePathDistribution);

        public ICommand ShowSettingsCommand => new ShowSettingsCommand(Config, OsHandler);
        public ICommand ShowExportDialogCommand => new ShowExportDialogDistributionCommand(this);
        public ICommand ShowImportDialogCommand => new ShowImportDialogCommand(this);

        public ICommand StartWslServiceCommand => new StartWslServiceCommand();
        public ICommand StopWslServiceCommand => new StopWslServiceCommand();
        public ICommand RestartWslServiceCommand => new RestartWslServiceCommand();

        public ICommand ShowSelectDialogCommand => new ShowSelectDistributionDialogCommand();

        public ICommand StartOnBoot => new RelayCommand(null, o => false);

        public ICommand OpenLogFileCommand => new OpenLogFileCommand();
        public ICommand CopyToClipboardCommand => new CopyToClipboardCommand();

        public ICommand OpenDistributionShell => new OpenShellDistributionCommand(SelectedDistribution);

        public DistributionClass SelectedDistribution { get; set; }
        private bool CanStartDistribution { get; set; } = true;
        private bool CanStopDistribution { get; set; } = true;
        private bool CanRenameDistribution { get; set; } = true;
        private bool CanChangeBasePathDistribution { get; } = false;

        private bool CanOpenBasePathDistribution(object arg)
        {
            if (arg == null) return false;

            var dist = (DistributionClass) arg;
            return Directory.Exists(dist.BasePathLocal);
        }

        private bool CanSetDefaultDistribution(object arg)
        {
            if (arg == null) return false;

            var dist = (DistributionClass) arg;
            return !dist.IsDefault;
        }

        private void DebugMode()
        {
            Debug.WriteLine("Debug mode enabled.");
            _view.Title = $"{_view.Title} - Debug Mode";
            Config.Configuration.MinimumLogLevel = LogEventLevel.Verbose;
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

        public bool ShowUnsupportedOsMessage()
        {
            return OsHandler.State == OsHandler.States.Unsupported && !Config.Configuration.HideUnsupportedOsMessage;
        }

        public bool ShowMinimumOsMessage()
        {
            return OsHandler.State == OsHandler.States.Minimum && Config.Configuration.ShowMinimumOsMessage;
        }

        private async void StartDistribution(object parameter)
        {
            CanStartDistribution = false;
            _ = await ToolboxClass.StartDistribution((DistributionClass) parameter);
            CanStartDistribution = true;
        }

        private async void StopDistribution(object parameter)
        {
            CanStopDistribution = false;
            _ = await ToolboxClass.TerminateDistribution((DistributionClass) parameter);
            CanStopDistribution = true;
        }

        private async void SetDefaultDistribution(object parameter)
        {
            await ToolboxClass.SetDefaultDistribution((DistributionClass) parameter);
        }

        private async void RenameDistribution(object parameter)
        {
            CanRenameDistribution = false;
            var newName = await _view.ShowInputAsync(
                "Rename",
                "Enter a new name for this distribution. The distribution will be restarted after renaming.\n\n" +
                "You can only use alphanumeric characters and must contain 3 characters or more. Spaces are not allowed.");

            if (newName is not null)
                try
                {
                    if (newName.All(char.IsLetterOrDigit) && newName.Length >= 3)
                    {
                        ToolboxClass.RenameDistribution((DistributionClass) parameter, newName);
                        await _view.ShowMessageAsync("Rename", $"Distribution has been renamed to {newName}.");
                    }
                    else
                    {
                        await _view.ShowMessageAsync(Resources.ERROR, Resources.ERROR_ONLY_ALPHANUMERIC);
                    }
                }
                catch (Exception e)
                {
                    await _view.ShowMessageAsync(Resources.ERROR, e.Message);
                }

            CanRenameDistribution = true;
        }

        private static void ChangeBasePathDistribution(object parameter)
        {
        }

        private async void RefreshDistributions()
        {
            DistributionList = await ToolboxClass
                .ListDistributions(Config.Configuration.HideDockerDistributions).ConfigureAwait(true);
        }

        private void RefreshDistributionsView(object parameter)
        {
            RefreshDistributions();
            _view.PopulateWsl();
        }

        private static void OpenBasePathDistribution(object parameter)
        {
            var distribution = (DistributionClass) parameter;

            if (!Directory.Exists(distribution.BasePathLocal)) return;

            ExplorerHelper.OpenLocal(distribution.BasePathLocal);
        }

        private void ShowApplication(object o)
        {
            _view.WindowState = WindowState.Normal;
            _view.Show();
        }

        private void SaveSuccessfullyEvent(object sender, EventArgs e)
        {
            Log().Debug("Configuration file saved, reloading configuration");
            _view.HandleConfiguration();
        }
    }
}