using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommandLine;
using Serilog.Core;
using Serilog.Events;
using WslToolbox.Core;
using WslToolbox.Core.Commands.Service;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Commands.Distribution;
using WslToolbox.Gui.Commands.Service;
using WslToolbox.Gui.Commands.Settings;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.Properties;
using WslToolbox.Gui.Views;
using CoreCommands = WslToolbox.Core.Commands;
using static WslToolbox.Gui.Handlers.LogHandler;
using EnableWindowsComponentsCommand = WslToolbox.Gui.Commands.Service.EnableWindowsComponentsCommand;

namespace WslToolbox.Gui.ViewModels
{
    public class Options
    {
        [Option('r', "reset", Default = false, HelpText = "Reset configuration")]
        public bool ResetConfiguration { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly OsHandler _osHandler = new();
        private readonly UpdateHandler _updateHandler;
        private readonly MainView _view;
        public readonly ICommand CheckForUpdates;
        public readonly ConfigurationHandler Config = new();

        public readonly ICommand EnableWindowsComponents = new EnableWindowsComponentsCommand();
        public readonly Logger Log;

        private List<DistributionClass> _distributionList = new();
        private BindingList<DistributionClass> _gridList = new();
        private DistributionClass _selectedDistribution;
        private bool _updateAvailable;
        private Visibility _updateAvailableVisibility = Visibility.Collapsed;
        public List<DistributionClass> InstallableDistributions = null;
        public KeyboardShortcutHandler KeyboardShortcutHandler;


        public MainViewModel(MainView view)
        {
            var args = Environment.GetCommandLineArgs();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(commandLineArgument =>
                {
                    if (commandLineArgument.ResetConfiguration) Config.Reset();
                });

            Log = Log();
            _view = view;
            _updateHandler = new UpdateHandler(_view);
            CheckForUpdates = new CheckForUpdateCommand(_updateHandler);

            if (AppConfiguration.DebugMode) InitializeDebugMode();
            InitializeKeyboardShortcuts();
            InitializeEventHandlers();
            InitializeUpdater();
        }

        public bool UpdateAvailable
        {
            get => _updateAvailable;
            set
            {
                if (_updateAvailable == value) return;
                _updateAvailable = value;
                UpdateAvailableVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(UpdateAvailable));
            }
        }

        public Visibility UpdateAvailableVisibility
        {
            get => _updateAvailableVisibility;
            set
            {
                _updateAvailableVisibility = value;
                OnPropertyChanged(nameof(UpdateAvailableVisibility));
            }
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
        public ICommand ExitApplication => new ExitApplicationCommand();
        public ICommand Refresh => new RefreshDistributionsCommand(_view);
        public ICommand ShowSettings => new ShowSettingsCommand(Config, _osHandler, KeyboardShortcutHandler);
        public ICommand ShowExportDialog => new ExportDistributionCommand(SelectedDistribution, this);
        public ICommand ShowImportDialog => new ImportDistributionCommand(SelectedDistribution, this);
        public ICommand StartWslService => new StartWslServiceCommand();
        public ICommand StopWslService => new StopWslServiceCommand();
        public ICommand RestartWslService => new RestartWslServiceCommand();
        public ICommand UpdateWslService => new UpdateWslServiceCommand();
        public ICommand ShowInstallDistributionDialog => new InstallDistributionCommand(this);
        public ICommand ShowAboutDialog => new ShowAboutDialogCommand(this);
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

        public void InitializeKeyboardShortcuts()
        {
            KeyboardShortcutHandler =
                new KeyboardShortcutHandler(Config.Configuration.KeyboardShortcutConfiguration, this);
        }

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
            Config.ConfigurationUpdatedSuccessfully += OnSaveSuccessfully;
            ToolboxClass.RefreshRequired += OnRefreshRequired;
            UpdateHandler.UpdateStatusReceived += OnUpdateStatusReceived;
            ShortcutHandler();
        }

        private async void OnUpdateStatusReceived(object sender, UpdateStatusArgs e)
        {
            if (e.UpdateError != null)
            {
                UpdateAvailable = false;
                Log().Error(e.UpdateError, Resources.ERROR_UPDATE_GENERIC);
            }

            UpdateAvailable = e.UpdateAvailable;
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
                    $"Version {e.CurrentVersion} is now available.");
        }

        private void ShortcutHandler()
        {
            _view.KeyUp += (_, args) =>
            {
                if (!Config.Configuration.KeyboardShortcutConfiguration.Enabled) return;

                var modifierKey = ModifierKeys.None;

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    modifierKey = ModifierKeys.Control;
                if (Keyboard.IsKeyDown(Key.LeftAlt))
                    modifierKey = ModifierKeys.Alt;
                if (Keyboard.IsKeyDown(Key.LeftShift))
                    modifierKey = ModifierKeys.Shift;


                var shortcut = KeyboardShortcutHandler.ShortcutByKey(args.Key, modifierKey);

                if (shortcut is not {Enabled: true}) return;
                shortcut.Action?.Invoke();
            };
        }

        private void OnRefreshRequired(object sender, EventArgs e)
        {
            Refresh.Execute(_view);
        }

        private async void InitializeUpdater()
        {
            if (!Config.Configuration.AutoCheckUpdates) return;

            await Task.Delay(5000);
            CheckForUpdates.Execute(false);
        }

        private void InitializeDebugMode()
        {
            Debug.WriteLine("Debug mode enabled.");
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
            return _osHandler.State == OsHandler.States.Unsupported && !Config.Configuration.HideUnsupportedOsMessage;
        }

        public bool ShowMinimumOsMessage()
        {
            return _osHandler.State == OsHandler.States.Minimum && Config.Configuration.ShowMinimumOsMessage;
        }

        public async void RefreshDistributions()
        {
            DistributionList = await ListServiceCommand
                .ListDistributions(Config.Configuration.HideDockerDistributions)
                .ConfigureAwait(true);
        }

        private void OnSaveSuccessfully(object sender, EventArgs e)
        {
            _view.InitializeDataGrid();
            _view.HandleConfiguration();

            Refresh.Execute(_view);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != nameof(DistributionList)) return;
            GridList = new BindingList<DistributionClass>(DistributionList
                .FindAll(x => x.IsInstalled)
            );

            _view.GridView.Visibility = GridList.Count == 0
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}