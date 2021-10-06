using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Serilog.Core;
using WslToolbox.Core;
using WslToolbox.Gui.Classes;
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
            InitializeComponent();
            InitializeViewModel();
            InitializeBindings();
            PopulateWsl();
            HandleConfiguration();
        }

        private void InitializeBindings()
        {
            BindElement[] mainViewBindings =
            {
                new(ToolboxSettings, ButtonBase.CommandProperty, "ShowSettingsCommand", DataContext)
            };

            BindHelper.AddBindings(mainViewBindings);
        }

        private void InitializeViewModel()
        {
            MainViewModel viewModel = new(this);

            DataContext = viewModel;
            _viewModel = viewModel;
            _log = _viewModel.Log;

            if (_viewModel.ShowUnsupportedOsMessage()) ShowOsUnsupportedMessage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _systemTray.Dispose();

            base.OnClosing(e);
        }

        private async void ShowOsUnsupportedMessage()
        {
            var notSupportedMessageDialogResult = await this.ShowMessageAsync("Warning",
                string.Format(
                    Properties.Resources.OS_NOT_SUPPORTED, Environment.OSVersion.Version.Build,
                    AppConfiguration.AppName) +
                Environment.NewLine + string.Format(
                    Properties.Resources.OS_NOT_SUPPORTED_BUILD_REQUIRED, AppConfiguration.AppMinimalOsBuild),
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

        private void DistroDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DistroDetails.ContextMenu = _viewModel.SelectedDistribution != null
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
            _systemTray.Tray.TrayMouseDoubleClick += (sender, args) => _viewModel.ShowApplicationCommand.Execute(null);

            _systemTray.Tray.ContextMenu = new ContextMenu
            {
                ItemsSource = _viewModel.SystemTrayMenuItems()
            };
        }

        private async void PopulateWsl()
        {
            var distroList = await ToolboxClass
                .ListDistributions(_viewModel.Config.Configuration.HideDockerDistributions).ConfigureAwait(true);

            DistroDetails.ItemsSource = distroList.FindAll(x => x.IsInstalled);
            DefaultDistribution.Content = ToolboxClass.DefaultDistribution().Name;
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeToTray && _viewModel.Config.Configuration.EnableSystemTray)
                ShowInTaskbar = WindowState != WindowState.Minimized;
        }
    }
}