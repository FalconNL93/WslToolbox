﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommandLine;
using Serilog.Core;
using Serilog.Events;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Core.Commands.Service;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Commands.Distribution;
using WslToolbox.Gui.Commands.Service;
using WslToolbox.Gui.Commands.Settings;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.Properties;
using WslToolbox.Gui.Views;
using static WslToolbox.Gui.Handlers.LogHandler;
using OpenShellDistributionCommand = WslToolbox.Gui.Commands.Distribution.OpenShellDistributionCommand;
using RenameDistributionCommand = WslToolbox.Gui.Commands.Distribution.RenameDistributionCommand;
using SetDefaultDistributionCommand = WslToolbox.Gui.Commands.Distribution.SetDefaultDistributionCommand;
using StartDistributionCommand = WslToolbox.Gui.Commands.Distribution.StartDistributionCommand;

namespace WslToolbox.Gui.ViewModels
{
    public class Options
    {
        [Option('d', "debug", Default = false, HelpText = "Enable debug mode")]
        public bool DebugMode { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Timer _statusPoller = new();
        private readonly UpdateHandler _updateHandler;
        private readonly MainView _view;
        public readonly ICommand CheckForUpdates;

        public readonly ConfigurationHandler Config = new();
        public readonly Logger Log;
        private readonly OsHandler OsHandler = new();

        private List<DistributionClass> _distributionList = new();

        private BindingList<DistributionClass> _gridList = new();

        private DistributionClass _selectedDistribution;

        public MainViewModel(MainView view)
        {
            var args = Environment.GetCommandLineArgs();
            Log = Log();
            _view = view;
            _updateHandler = new UpdateHandler(_view);
            CheckForUpdates = new CheckForUpdateCommand(_updateHandler);

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    if (o.DebugMode) DebugMode();
                });

            InitializeEventHandlers();
            InitializeStatusPoller();
            InitializeUpdater();
        }

        public List<DistributionClass> DistributionList
        {
            get => _distributionList;
            set
            {
                _distributionList = value;

                OnPropertyChanged(nameof(DistributionList));
            }
        }

        public BindingList<DistributionClass> GridList
        {
            get => _gridList;
            set
            {
                _gridList = value;

                OnPropertyChanged(nameof(GridList));
            }
        }

        public DistributionClass SelectedDistribution
        {
            get => _selectedDistribution;
            set
            {
                _selectedDistribution = value;

                OnPropertyChanged(nameof(SelectedDistribution));
            }
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
        public ICommand UpdateWslService => new UpdateWslServiceCommand();
        public ICommand ShowSelectDialog => new ShowSelectDistributionDialogCommand();
        public ICommand OpenLogFile => new OpenLogFileCommand();
        public ICommand CopyToClipboard => new CopyToClipboardCommand();
        public ICommand OpenDistributionShell => new OpenShellDistributionCommand(SelectedDistribution);
        public ICommand RenameDistribution => new RenameDistributionCommand(SelectedDistribution);
        public ICommand StartDistribution => new StartDistributionCommand(SelectedDistribution);
        public ICommand StopDistribution => new StopDistributionCommand(SelectedDistribution);
        public ICommand SetDefaultDistribution => new SetDefaultDistributionCommand(SelectedDistribution);
        public ICommand OpenBasePathDistribution => new OpenBasePathDistribution(SelectedDistribution);
        public ICommand DeleteDistribution => new DeleteDistributionCommand(SelectedDistribution, _view);

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetStatus(string status)
        {
            var topMenuBar = (ItemsControl) _view.FindName("TopMenu");
            if (topMenuBar == null) return;

            foreach (var topMenuBarItem in topMenuBar.Items)
            {
                if (topMenuBarItem.GetType() != typeof(Label)) continue;
                var topMenuBarItemLabel = (Label) topMenuBarItem;

                topMenuBarItemLabel.Content = status;
            }
        }

        private void InitializeEventHandlers()
        {
            _statusPoller.Elapsed += StatusPollerEventHandler;
            Config.ConfigurationUpdatedSuccessfully += SaveSuccessfullyEvent;

            ExportDistributionCommand.DistributionExportStarted +=
                DistributionChangedEventHandler;
            ImportDistributionCommand.DistributionImportStarted +=
                DistributionChangedEventHandler;
            Core.Commands.Distribution.RenameDistributionCommand.DistributionRenameStarted +=
                DistributionChangedEventHandler;
            Core.Commands.Distribution.SetDefaultDistributionCommand.DistributionDefaultSet +=
                DistributionChangedEventHandler;
            Core.Commands.Distribution.StartDistributionCommand.DistributionStartFinished +=
                DistributionChangedEventHandler;
            TerminateDistributionCommand.DistributionTerminateFinished +=
                DistributionChangedEventHandler;
            UnregisterDistributionCommand.DistributionUnregisterFinished +=
                DistributionChangedEventHandler;

            UpdateHandler.UpdateStatusReceived += OnUpdateStatusReceived;

            if (!Config.Configuration.DisableShortcuts) ShortcutHandler();
        }

        private async void OnUpdateStatusReceived(object sender, UpdateStatusArgs e)
        {
            if (e.UpdateError != null)
                Log().Error(e.UpdateError, Resources.ERROR_UPDATE_GENERIC);

            if (!e.UpdateAvailable)
            {
                Log().Information("No update available");
                if (e.ShowPrompt && e.UpdateError == null)
                    await DialogHelper.ShowMessageBoxInfo(
                        "Update",
                        "You are running the latest version.",
                        closeButtonText: "Close", dialogOwner: _view).ShowAsync();

                return;
            }

            Log().Information("Version {CurrentVersion} is available", e.CurrentVersion);
            if (e.ShowPrompt)
                _updateHandler.ShowUpdatePrompt();
            else if (_view.SystemTray.Tray != null &&
                     Config.Configuration.NotificationConfiguration.NewVersionAvailable)
                _view.SystemTray.ShowNotification("Update available",
                    $"Version {e.CurrentVersion} now available to install.");
        }

        private void ShortcutHandler()
        {
            _view.KeyUp += (sender, args) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    switch (args.Key)
                    {
                        case Key.OemComma:
                            ShowSettings.Execute(null);
                            break;
                        case Key.Q:
                            ExitApplication.Execute(null);
                            break;
                    }

                if (args.Key == Key.F5) Refresh.Execute(_view);
            };
        }

        private void DistributionChangedEventHandler(object sender, EventArgs e)
        {
            Refresh.Execute(_view);
        }

        private void InitializeStatusPoller()
        {
            _statusPoller.Enabled = Config.Configuration.EnableServicePolling;
            _statusPoller.Interval = Config.Configuration.ServicePollingInterval;
        }

        private async void InitializeUpdater()
        {
            if (!Config.Configuration.AutoCheckUpdates) return;

            await Task.Delay(5000);
            CheckForUpdates.Execute(false);
        }

        private void StatusPollerEventHandler(object sender, ElapsedEventArgs e)
        {
            Refresh.Execute(_view);
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
            DistributionList = await ListServiceCommand
                .ListDistributions(Config.Configuration.HideDockerDistributions)
                .ConfigureAwait(true);
        }

        private void SaveSuccessfullyEvent(object sender, EventArgs e)
        {
            _view.HandleConfiguration();
            Refresh.Execute(_view);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(DistributionList))
            {
                GridList = new BindingList<DistributionClass>(DistributionList
                    .FindAll(x => x.IsInstalled)
                );

                _view.GridView.Visibility = GridList.Count == 0
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }
    }
}