using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModernWpf;
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
    public partial class MainView
    {
        public readonly SystemTrayHelper SystemTray = new();
        private Logger _log;
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeViewModel();
            WslIsEnabledCheck();
            InitializeComponent();
            InitializeDataGrid();
            InitializeContextMenus();
            InitializeTopMenu();
            HandleConfiguration();

            PopulateWsl();
        }

        private void InitializeDataGrid()
        {
            var abc = new ObservableCollection<DataGridColumn>();

            GridView.Children.Add(
                UiElementHelper.AddDistributionDataGrid(
                    nameof(_viewModel.GridList),
                    _viewModel
                )
            );
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
            // DistributionDetails.MouseDoubleClick += DistributionDetailsOnMouseDoubleClick;
            // DistributionDetails.MouseLeftButtonUp += DistributionDetailsOnMouseSingleClick;
        }

        private void InitializeTopMenu()
        {
            TopMenu.ItemsSource = UiElementHelper.ItemsListGroup(TopMenuCollection.Items(_viewModel));
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

            // if (_viewModel.Config.Configuration.GridConfiguration.SingleClick ==
            //     GridConfiguration.GridConfigurationOpenContextMenu)
            //     DistributionDetails.ContextMenu.IsOpen = true;
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

            SystemTray.Initialize(_viewModel.Config.Configuration.EnableSystemTray
                ? Visibility.Visible
                : Visibility.Hidden);

            if (_viewModel.Config.Configuration.MinimizeOnStartup)
            {
                WindowState = WindowState.Minimized;
                Hide();
            }

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
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeToTray && _viewModel.Config.Configuration.EnableSystemTray)
                ShowInTaskbar = WindowState != WindowState.Minimized;
        }
    }
}