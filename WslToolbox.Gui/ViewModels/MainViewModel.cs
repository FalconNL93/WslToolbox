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

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.DebugMode) DebugMode();
                });

            InitializeEventHandlers();
            InitializeStatusPoller();
        }

        public ICommand ShowApplication => new ShowApplicationCommand(_view);
        public ICommand NotImplemented => new NotImplementedCommand();
        public ICommand SaveConfigurationCommand => new SaveSettingsCommand(Config);
        public ICommand ExitApplication => new ExitApplicationCommand();
        public ICommand Refresh => new RefreshDistributionsCommand(_view);
        public ICommand ShowSettings => new ShowSettingsCommand(Config, OsHandler);
        public ICommand ShowExportDialog => new ShowExportDialogDistributionCommand(SelectedDistribution);
        public ICommand ShowImportDialog => new ShowImportDialogCommand(SelectedDistribution);
        public ICommand StartWslService => new StartWslServiceCommand();
        public ICommand StopWslService => new StopWslServiceCommand();
        public ICommand RestartWslService => new RestartWslServiceCommand();
        public ICommand ShowSelectDialog => new ShowSelectDistributionDialogCommand();
        public ICommand OpenLogFile => new OpenLogFileCommand();
        public ICommand CopyToClipboard => new CopyToClipboardCommand();
        public ICommand OpenDistributionShell => new OpenShellDistributionCommand(SelectedDistribution);
        public ICommand RenameDistribution => new RenameDistributionCommand(SelectedDistribution, _view);
        public ICommand StartDistribution => new StartDistributionCommand(SelectedDistribution);
        public ICommand StopDistribution => new StopDistributionCommand(SelectedDistribution);
        public ICommand SetDefaultDistribution => new SetDefaultDistributionCommand(SelectedDistribution);
        public ICommand OpenBasePathDistribution => new OpenBasePathDistribution(SelectedDistribution);
        public DistributionClass SelectedDistribution { get; set; }

        private void InitializeEventHandlers()
        {
            _statusPoller.Elapsed += StatusPollerEventHandler;
            Config.ConfigurationUpdatedSuccessfully += SaveSuccessfullyEvent;
            StartDistributionCommand.DistributionStarted += DistributionChangedEventHandler;
            StopDistributionCommand.DistributionStopped += DistributionChangedEventHandler;
            RenameDistributionCommand.DistributionRenamed += DistributionChangedEventHandler;
            SetDefaultDistributionCommand.DistributionDefaultChanged += DistributionChangedEventHandler;
        }

        private void DistributionChangedEventHandler(object sender, EventArgs e)
        {
            _view.PopulateWsl();
        }

        private void InitializeStatusPoller()
        {
            _statusPoller.Enabled = Config.Configuration.EnableServicePolling;
            _statusPoller.Interval = Config.Configuration.ServicePollingInterval;
        }

        private void StatusPollerEventHandler(object sender, ElapsedEventArgs e)
        {
            _view.PopulateWsl();
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