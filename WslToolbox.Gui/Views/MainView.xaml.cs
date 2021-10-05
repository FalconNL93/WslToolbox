using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Serilog.Core;
using WslToolbox.Core;
using WslToolbox.Gui.Classes;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private Logger _log;
        private readonly SystemTrayClass _systemTray = new();
        private MainViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();
            InitializeViewModel();
            PopulateWsl();
            HandleConfiguration();
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
            await this.ShowMessageAsync("Warning",
                $"Your current operating system is not supported by {AppConfiguration.AppName}");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void DistroConvert_Click(object sender, RoutedEventArgs e)
        {
            var command = await ToolboxClass.ConvertDistribution(_viewModel.SelectedDistribution).ConfigureAwait(true);
            var output = Regex.Replace(command.Output, "\t", " ");
            MessageBox.Show(output, "Convert", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private async void DistroExport_Click(object sender, RoutedEventArgs e)
        {
            var saveExportDialog = FileDialogHandler.SaveFileDialog();

            if (!(bool) saveExportDialog.ShowDialog()) return;

            var fileName = saveExportDialog.FileName;

            try
            {
                await ToolboxClass.ExportDistribution(_viewModel.SelectedDistribution, fileName).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DistroImport_Click(object sender, RoutedEventArgs e)
        {
            var openExportDialog = FileDialogHandler.OpenFileDialog();

            if (!(bool) openExportDialog.ShowDialog()) return;

            var fileName = openExportDialog.FileName;

            ImportView importDistroWindow = new(fileName);
            importDistroWindow.ShowDialog();

            if (!(bool) importDistroWindow.DialogResult) return;

            try
            {
                await ToolboxClass.ImportDistribution(_viewModel.SelectedDistribution, importDistroWindow.DistroName,
                    importDistroWindow.DistroSelectedDirectory, fileName).ConfigureAwait(true);

                PopulateWsl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DistroInstall_Click(object sender, RoutedEventArgs e)
        {
            SelectDistroView selectDistroWindow = new();

            var distroSelected = selectDistroWindow.ShowDialog();

            if (!(bool) distroSelected) return;

            var selectedDistro = (DistributionClass) selectDistroWindow.AvailableDistros.SelectedItem;
            ToolboxClass.ShellDistribution(selectedDistro);
        }

        private async void DistroRestart_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.TerminateDistribution(_viewModel.SelectedDistribution).ConfigureAwait(true);
            _ = await ToolboxClass.StartDistribution(_viewModel.SelectedDistribution).ConfigureAwait(true);

            PopulateWsl();
        }

        private async void DistroSetDefault_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.SetDefaultDistribution(_viewModel.SelectedDistribution).ConfigureAwait(true);

            PopulateWsl();
        }

        private void DistroShell_Click(object sender, RoutedEventArgs e)
        {
            ToolboxClass.ShellDistribution(_viewModel.SelectedDistribution);
        }
        
        private async void DistroUninstall_Click(object sender, RoutedEventArgs e)
        {
            var uninstallMessagebox = await this.ShowMessageAsync("Uninstall?",
                $"Are you sure you want to uninstall {_viewModel.SelectedDistribution.Name}? This will also destroy all data within the distribution.",
                MessageDialogStyle.AffirmativeAndNegative);

            if (uninstallMessagebox != MessageDialogResult.Affirmative) return;

            _log.Information($"Uninstalling distribution {_viewModel.SelectedDistribution.Name}");
            _ = await ToolboxClass.UnregisterDistribution(_viewModel.SelectedDistribution).ConfigureAwait(true);

            PopulateWsl();
        }

        public void HandleConfiguration()
        {
            HandleSystemTray();

            ThemeHandler.Set(_viewModel.Config.Configuration.SelectedStyle);
            _log.Debug("Configuration file applied");
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

        private void RefreshWsl_Click(object sender, RoutedEventArgs e)
        {
            PopulateWsl();
        }

        private async void RestartWsl_Click(object sender, RoutedEventArgs e)
        {
            await ToolboxClass.StopWsl().ConfigureAwait(true);
            await ToolboxClass.StartWsl().ConfigureAwait(true);
        }

        private async void StartWsl_Click(object sender, RoutedEventArgs e)
        {
            await ToolboxClass.StartWsl().ConfigureAwait(true);
        }

        private async void StatusWsl_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.StatusWsl().ConfigureAwait(true);

            PopulateWsl();
        }

        private async void StopWsl_Click(object sender, RoutedEventArgs e)
        {
            await ToolboxClass.StopWsl().ConfigureAwait(true);
        }

        private async void UpdateWsl_Click(object sender, RoutedEventArgs e)
        {
            UpdateWsl.IsEnabled = false;
            var command = await ToolboxClass.UpdateWsl().ConfigureAwait(true);
            var output = Regex.Replace(command.Output, "\t", " ");

            if (output.Contains("No updates are available."))
                _ = await this.ShowMessageAsync("WSL Update", "No updates are available.");

            UpdateWsl.IsEnabled = true;
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (_viewModel.Config.Configuration.MinimizeToTray && _viewModel.Config.Configuration.EnableSystemTray)
                ShowInTaskbar = WindowState != WindowState.Minimized;
        }
    }
}