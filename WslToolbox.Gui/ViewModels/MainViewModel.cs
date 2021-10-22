﻿using System;
using System.Diagnostics;
using System.IO;
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
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
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
        private readonly bool _osSupported = OsHandler.Supported();
        private readonly MainView _view;
        public readonly ConfigurationHandler Config = new();
        public readonly Logger Log;

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
        }

        public ICommand ShowApplicationCommand =>
            new RelayCommand(ShowApplication, o => _view.WindowState == WindowState.Minimized);

        public ICommand NotImplementedCommand => new RelayCommand(o => { }, o => false);
        public ICommand SaveConfigurationCommand => new SaveSettingsCommand(Config);
        public ICommand ExitApplicationCommand => new RelayCommand(o => { Environment.Exit(-1); }, o => true);
        public ICommand StartWslServiceCommand => new RelayCommand(StartWslService, o => CanStartWslService);
        public ICommand StopWslServiceCommand => new RelayCommand(StopWslService, o => CanStopWslService);
        public ICommand RestartWslServiceCommand => new RelayCommand(RestartWslService, o => true);
        public ICommand StartDistributionCommand => new RelayCommand(StartDistribution, o => CanStartDistribution);
        public ICommand StopDistributionCommand => new RelayCommand(StopDistribution, o => CanStopDistribution);
        public ICommand RenameDistributionCommand => new RelayCommand(RenameDistribution, o => CanRenameDistribution);

        public ICommand ChangeBasePathDistributionCommand =>
            new RelayCommand(ChangeBasePathDistribution, o => CanChangeBasePathDistribution);

        public ICommand OpenBasePathDistributionCommand =>
            new RelayCommand(OpenBasePathDistribution, CanOpenBasePathDistribution);

        public ICommand ShowSettingsCommand => new ShowSettingsCommand(Config);
        public ICommand ShowExportDialogCommand => new ShowExportDialogCommand(this);
        public ICommand ShowImportDialogCommand => new RelayCommand(ShowImportDialog, o => CanImportDistribution);

        public ICommand StartOnBoot => new RelayCommand(null, o => false);

        public ICommand OpenLogFileCommand => new OpenLogFileCommand();

        public DistributionClass SelectedDistribution { get; set; }

        private bool CanStartWslService { get; set; } = true;
        private bool CanStopWslService { get; set; } = true;
        private bool CanStartDistribution { get; set; } = true;
        private bool CanStopDistribution { get; set; } = true;
        private bool CanRenameDistribution { get; set; } = true;
        private bool CanChangeBasePathDistribution { get; } = false;

        private bool CanImportDistribution { get; set; } = true;

        private bool CanOpenBasePathDistribution(object arg)
        {
            if (arg == null) return false;

            var dist = (DistributionClass) arg;
            return Directory.Exists(dist.BasePathLocal);
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

        private async void ShowImportDialog(object parameter)
        {
            var openExportDialog = FileDialogHandler.OpenFileDialog();

            if (!(bool) openExportDialog.ShowDialog()) return;

            var fileName = openExportDialog.FileName;

            ImportView importDistroWindow = new(fileName);
            importDistroWindow.ShowDialog();

            if (!(bool) importDistroWindow.DialogResult) return;

            try
            {
                CanImportDistribution = false;
                await ToolboxClass.ImportDistribution(SelectedDistribution, importDistroWindow.DistroName,
                    importDistroWindow.DistroSelectedDirectory, fileName).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            CanImportDistribution = true;
        }

        private async void StartWslService(object parameter)
        {
            CanStartWslService = false;
            _ = await ToolboxClass.StartWsl().ConfigureAwait(true);
            CanStartWslService = true;
        }

        public bool ShowUnsupportedOsMessage()
        {
            return !_osSupported && !Config.Configuration.HideUnsupportedOsMessage;
        }

        private async void StopWslService(object parameter)
        {
            CanStopWslService = false;
            _ = await ToolboxClass.StopWsl().ConfigureAwait(true);
            CanStopWslService = true;
        }

        private void RestartWslService(object parameter)
        {
            StopWslServiceCommand.Execute(null);
            StartWslServiceCommand.Execute(null);
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

        private async void RenameDistribution(object parameter)
        {
            CanRenameDistribution = false;
            var newName = await _view.ShowInputAsync(
                "Rename",
                "Enter a new name for this distribution. The distribution will be restarted after renaming.");
            try
            {
                ToolboxClass.RenameDistribution((DistributionClass) parameter, newName);
                await _view.ShowMessageAsync("Rename", $"Distribution has been renamed to {newName}.");
            }
            catch (Exception e)
            {
                await _view.ShowMessageAsync("Error", e.Message);
            }

            CanRenameDistribution = true;
        }

        private static void ChangeBasePathDistribution(object parameter)
        {
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