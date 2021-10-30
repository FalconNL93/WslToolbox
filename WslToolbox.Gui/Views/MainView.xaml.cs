using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Serilog.Core;
using WslToolbox.Core;
using WslToolbox.Gui.Classes;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private readonly SystemTrayClass _systemTray = new();
        private Logger _log;
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeViewModel();
            WslIsEnabledCheck();
            InitializeComponent();
            InitializeBindings();
            InitializeContextMenus();
            HandleConfiguration();

            PopulateWsl();
        }

        private static void WslIsEnabledCheck()
        {
            var system32 = Environment.SystemDirectory;
            if (File.Exists($@"{system32}\wsl.exe")) return;

            var messageBoxResult =
                MessageBox.Show("WSL does not appear to be installed on your system. Do you want to enable WSL?",
                    Properties.Resources.ERROR, MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes) ToolboxClass.EnableWslComponent();

            Environment.Exit(1);
        }

        private void InitializeBindings()
        {
            BindElement[] mainViewBindings =
            {
                // Service
                new(StartWsl, ButtonBase.CommandProperty, nameof(_viewModel.StartWslService), DataContext),
                new(StopWsl, ButtonBase.CommandProperty, nameof(_viewModel.StopWslService), DataContext),
                new(RestartWsl, ButtonBase.CommandProperty, nameof(_viewModel.RestartWslService), DataContext),
                new(UpdateWsl, ButtonBase.CommandProperty, nameof(_viewModel.NotImplemented), DataContext),
                new(RefreshWsl, ButtonBase.CommandProperty, nameof(_viewModel.Refresh), DataContext),

                // Other
                new(ToolboxSettings, ButtonBase.CommandProperty, nameof(_viewModel.ShowSettings), DataContext),
                new(ToolboxOutput, ButtonBase.CommandProperty, nameof(_viewModel.OpenLogFile), DataContext),
                new(ExitButton, ButtonBase.CommandProperty, nameof(_viewModel.ExitApplication), DataContext)
            };

            BindHelper.AddBindings(mainViewBindings);
        }

        private void InitializeContextMenus()
        {
            ControlMenuButton.ItemsSource = ManageMenuCollection.Items(_viewModel);
        }

        private void InitializeViewModel()
        {
            MainViewModel viewModel = new(this);

            DataContext = viewModel;
            _viewModel = viewModel;
            _log = _viewModel.Log;

            if (_viewModel.ShowUnsupportedOsMessage()) ShowOsUnsupportedMessage();
            if (_viewModel.ShowMinimumOsMessage()) ShowOsUnsupportedMessage(OsHandler.States.Minimum);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _systemTray.Dispose();

            base.OnClosing(e);
        }

        private async void ShowOsUnsupportedMessage(OsHandler.States state = OsHandler.States.Unsupported)
        {
            var osTitle = "Warning";
            var osMessage = string.Format(
                                Properties.Resources.OS_NOT_SUPPORTED, _viewModel.OsHandler.OsBuild,
                                AppConfiguration.AppName) +
                            Environment.NewLine + string.Format(
                                Properties.Resources.OS_NOT_SUPPORTED_BUILD_REQUIRED, OsHandler.MinimumOsBuild);

            if (state == OsHandler.States.Minimum)
            {
                osTitle = Properties.Resources.NOTICE;
                osMessage = string.Format(
                                Properties.Resources.OS_MINIMUM, _viewModel.OsHandler.OsBuild,
                                AppConfiguration.AppName) +
                            Environment.NewLine + string.Format(
                                Properties.Resources.OS_MINIMUM_BUILD, OsHandler.RecommendedOsBuild);
            }

            var notSupportedMessageDialogResult = await this.ShowMessageAsync(osTitle,
                osMessage,
                MessageDialogStyle.AffirmativeAndNegative,
                new MetroDialogSettings
                {
                    AffirmativeButtonText = "Close",
                    NegativeButtonText = "Continue anyway",
                    DefaultButtonFocus = MessageDialogResult.Affirmative
                }
            );

            if (notSupportedMessageDialogResult == MessageDialogResult.Affirmative) Environment.Exit(1);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void DistributionDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DistributionDetails.ContextMenu = _viewModel.SelectedDistribution != null
                ? new ContextMenu
                {
                    ItemsSource = _viewModel.DataGridMenuItems()
                }
                : null;
        }

        public void HandleConfiguration()
        {
            HandleSystemTray();

            ThemeHandler.Set(_viewModel.Config.Configuration.SelectedStyle);
        }

        private void HandleSystemTray()
        {
            _systemTray.Dispose();

            if (!_viewModel.Config.Configuration.EnableSystemTray) return;
            if (_viewModel.Config.Configuration.MinimizeOnStartup)
            {
                WindowState = WindowState.Minimized;
                Hide();
            }

            _systemTray.Show();
            _systemTray.Tray.TrayMouseDoubleClick += (sender, args) => _viewModel.ShowApplication.Execute(null);

            _systemTray.Tray.ContextMenu = new ContextMenu
            {
                ItemsSource = _viewModel.SystemTrayMenuItems()
            };
        }

        public void PopulateWsl()
        {
            if (_viewModel == null)
            {
                MessageBox.Show("Could not initialize ViewModel", "Error", MessageBoxButton.OK);
                Environment.Exit(1);
            }

            _viewModel.RefreshDistributions();
            var distributionList = _viewModel.DistributionList;

            if (distributionList != null)
                DistributionDetails.ItemsSource = distributionList.FindAll(x => x.IsInstalled);
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeToTray && _viewModel.Config.Configuration.EnableSystemTray)
                ShowInTaskbar = WindowState != WindowState.Minimized;
        }
    }
}