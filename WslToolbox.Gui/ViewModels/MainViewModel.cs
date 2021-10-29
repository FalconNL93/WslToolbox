using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows.Data;
using System.Windows.Input;
using CommandLine;
using Serilog.Core;
using Serilog.Events;
using WslToolbox.Core;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Commands.Distribution;
using WslToolbox.Gui.Commands.Service;
using WslToolbox.Gui.Commands.Settings;
using WslToolbox.Gui.Handlers;
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
        private readonly Timer _statusPoller = new();
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
            InitializeStatusPoller();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.DebugMode) DebugMode();
                });

            RefreshDistributions();
        }

        public ICommand ShowApplicationCommand => new ShowApplicationCommand(_view);
        public ICommand NotImplementedCommand => new NotImplementedCommand();
        public ICommand SaveConfigurationCommand => new SaveSettingsCommand(Config);
        public ICommand ExitApplicationCommand => new ExitApplicationCommand();
        public ICommand RefreshCommand => new RefreshDistributionsCommand(_view);
        public ICommand ShowSettingsCommand => new ShowSettingsCommand(Config, OsHandler);
        public ICommand ShowExportDialogCommand => new ShowExportDialogDistributionCommand(SelectedDistribution);
        public ICommand ShowImportDialogCommand => new ShowImportDialogCommand(SelectedDistribution);
        public ICommand StartWslServiceCommand => new StartWslServiceCommand();
        public ICommand StopWslServiceCommand => new StopWslServiceCommand();
        public ICommand RestartWslServiceCommand => new RestartWslServiceCommand();
        public ICommand ShowSelectDialogCommand => new ShowSelectDistributionDialogCommand();
        public ICommand OpenLogFileCommand => new OpenLogFileCommand();
        public ICommand CopyToClipboardCommand => new CopyToClipboardCommand();
        public ICommand OpenDistributionShell => new OpenShellDistributionCommand(SelectedDistribution);
        public ICommand RenameDistributionCommand => new RenameDistributionCommand(SelectedDistribution, _view);
        public ICommand StartDistributionCommand => new StartDistributionCommand(SelectedDistribution);
        public ICommand StopDistributionCommand => new StopDistributionCommand(SelectedDistribution);
        public ICommand SetDefaultDistributionCommand => new SetDefaultDistributionCommand(SelectedDistribution);
        public ICommand OpenBasePathDistributionCommand => new OpenBasePathDistribution(SelectedDistribution);
        public DistributionClass SelectedDistribution { get; set; }

        private void InitializeEventHandlers()
        {
            _statusPoller.Elapsed += StatusPollerEventHandler;
            Config.ConfigurationUpdatedSuccessfully += SaveSuccessfullyEvent;
        }

        private void InitializeStatusPoller()
        {
            _statusPoller.Interval = 5000;
            _statusPoller.Enabled = true;
        }

        private void StatusPollerEventHandler(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine("Polling...");
            RefreshDistributions();
            RefreshCommand.Execute(null);
        }

        private void DebugMode()
        {
            Debug.WriteLine("Debug mode enabled.");
            _view.Title = $"{_view.Title} - Debug Mode";
            Config.Configuration.MinimumLogLevel = LogEventLevel.Verbose;
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

        public async void RefreshDistributions()
        {
            DistributionList = await ToolboxClass
                .ListDistributions(Config.Configuration.HideDockerDistributions).ConfigureAwait(true);
        }

        private void SaveSuccessfullyEvent(object sender, EventArgs e)
        {
            Debug.WriteLine("Configuration file saved, reloading configuration");
            _view.HandleConfiguration();
        }
    }
}