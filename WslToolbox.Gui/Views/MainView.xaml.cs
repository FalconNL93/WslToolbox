using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf;
using ModernWpf.Controls;
using Serilog.Core;
using WslToolbox.Core.Commands.Distribution;
using WslToolbox.Core.Commands.Service;
using WslToolbox.Gui.Collections;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public readonly SystemTrayHelper SystemTray = new();
        private Logger _log;
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeViewModel();
            WslIsEnabledCheck();
            InitializeComponent();
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

            if (messageBoxResult == MessageBoxResult.Yes) EnableServiceCommand.Execute();

            Environment.Exit(1);
        }

        private void InitializeContextMenus()
        {
            var addControlMenuButtonFlyout = new MenuFlyout();
            var controlMenuButtonFlyout = new MenuFlyout();
            var serviceControlMenuButtonFlyout = new MenuFlyout();

            foreach (var menuItem in AddMenuCollection.Items(_viewModel))
                addControlMenuButtonFlyout.Items.Add(menuItem);

            foreach (var menuItem in ServiceMenuCollection.Items(_viewModel))
                serviceControlMenuButtonFlyout.Items.Add(menuItem);

            foreach (var menuItem in ManageMenuCollection.Items(_viewModel))
                controlMenuButtonFlyout.Items.Add(menuItem);

            AddControlMenuButton.Flyout = addControlMenuButtonFlyout;
            ControlMenuButton.Flyout = controlMenuButtonFlyout;
            ServiceControlMenuButton.Flyout = serviceControlMenuButtonFlyout;

            DistributionDetails.MouseDoubleClick += DistributionDetailsOnMouseDoubleClick;
            DistributionDetails.MouseLeftButtonUp += DistributionDetailsOnMouseSingleClick;
        }

        private void DistributionDetailsOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedDistribution == null) return;

            if (_viewModel.Config.Configuration.GridConfiguration.DoubleClick ==
                GridConfiguration.GridConfigurationOpenTerminal)
                OpenShellDistributionCommand.Execute(_viewModel.SelectedDistribution);
        }

        private void DistributionDetailsOnMouseSingleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedDistribution == null) return;

            if (_viewModel.Config.Configuration.GridConfiguration.SingleClick ==
                GridConfiguration.GridConfigurationOpenContextMenu)
                DistributionDetails.ContextMenu.IsOpen = true;
        }

        private void InitializeViewModel()
        {
            MainViewModel viewModel = new(this);

            DataContext = viewModel;
            _viewModel = viewModel;
            _log = _viewModel.Log;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeOnClose)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;

                return;
            }

            SystemTray.Dispose();

            base.OnClosing(e);
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
            HandleTheme();
            HandleSystemTray();
        }

        private void HandleTheme()
        {
            var selectedStyle = _viewModel.Config.Configuration.AppearanceConfiguration.SelectedStyle;

            ThemeManager.Current.ApplicationTheme = selectedStyle switch
            {
                ThemeConfiguration.Styles.Light => ApplicationTheme.Light,
                ThemeConfiguration.Styles.Dark => ApplicationTheme.Dark,
                _ => null
            };
        }

        private void HandleSystemTray()
        {
            SystemTray.Dispose();

            if (!_viewModel.Config.Configuration.EnableSystemTray) return;
            if (_viewModel.Config.Configuration.MinimizeOnStartup)
            {
                WindowState = WindowState.Minimized;
                Hide();
            }

            SystemTray.Show();
            SystemTray.Tray.TrayMouseDoubleClick += (sender, args) => _viewModel.ShowApplication.Execute(null);

            SystemTray.Tray.ContextMenu = new ContextMenu
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

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeToTray && _viewModel.Config.Configuration.EnableSystemTray)
                ShowInTaskbar = WindowState != WindowState.Minimized;
        }
    }
}