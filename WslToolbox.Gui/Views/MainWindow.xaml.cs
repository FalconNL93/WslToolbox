using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using WslToolbox.Core;
using WslToolbox.Gui.Classes;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly ConfigurationHandler Config = new();
        private readonly OutputWindow OutputWindow = new();
        private readonly SystemTrayClass SystemTray = new();

        public MainWindow()
        {
            InitializeComponent();
            PopulateWsl();
            PopulateSelectedDistro();
            HandleConfiguration();
        }

        private DistributionClass SelectedDistro { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void DistroConvert_Click(object sender, RoutedEventArgs e)
        {
            OutputWindow.WriteOutput($"Converting {SelectedDistro.Name} to WSL2...");
            CommandClass command = await ToolboxClass.ConvertDistribution(SelectedDistro).ConfigureAwait(true);
            string output = Regex.Replace(command.Output, "\t", " ");
            MessageBox.Show(output, "Convert", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DistroDetails_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedDistro = DistroDetails.SelectedItem is DistributionClass @class ? @class : null;
            PopulateSelectedDistro();
        }

        private async void DistroExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveExportDialog = new()
            {
                Title = "Export",
                Filter = "Tarball (*.tar)|*.tar|All files (*.*)|*.*",
                AddExtension = true,
                OverwritePrompt = true,
                DefaultExt = "tar",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (!(bool)saveExportDialog.ShowDialog())
            {
                return;
            }

            string fileName = saveExportDialog.FileName;

            try
            {
                OutputWindow.WriteOutput($"Exporting {SelectedDistro.Name} to {fileName}...");
                CommandClass command = await ToolboxClass.ExportDistribution(SelectedDistro, fileName).ConfigureAwait(true);
                OutputWindow.WriteOutput($"{SelectedDistro.Name} exported.");
            }
            catch (Exception ex)
            {
                OutputWindow.WriteOutput($"{SelectedDistro.Name} export failed.");
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DistroImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog saveExportDialog = new()
            {
                Title = "Export",
                Filter = "Tarball (*.tar)|*.tar|All files (*.*)|*.*",
                AddExtension = true,
                DefaultExt = "tar",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (!(bool)saveExportDialog.ShowDialog())
            {
                return;
            }

            string fileName = saveExportDialog.FileName;

            ImportDistroWindow importDistroWindow = new(fileName);
            importDistroWindow.ShowDialog();

            if (!(bool)importDistroWindow.DialogResult)
            {
                return;
            }

            try
            {
                OutputWindow.WriteOutput($"Importing {fileName} as {importDistroWindow.DistroName}...");
                CommandClass command = await ToolboxClass.ImportDistribution(SelectedDistro, importDistroWindow.DistroName, importDistroWindow.DistroSelectedDirectory, fileName).ConfigureAwait(true);
                OutputWindow.WriteOutput($"{importDistroWindow.DistroName} imported.");

                PopulateWsl();
            }
            catch (Exception ex)
            {
                OutputWindow.WriteOutput("Import failed.");
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DistroInstall_Click(object sender, RoutedEventArgs e)
        {
            SelectDistroWindow selectDistroWindow = new();

            bool? distroSelected = selectDistroWindow.ShowDialog();

            if ((bool)distroSelected)
            {
                DistributionClass selectedDistro = (DistributionClass)selectDistroWindow.AvailableDistros.SelectedItem;
                ToolboxClass.ShellDistribution(selectedDistro);
            }
        }

        private async void DistroRestart_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.TerminateDistribution(SelectedDistro).ConfigureAwait(true);
            _ = await ToolboxClass.StartDistribution(SelectedDistro).ConfigureAwait(true);

            PopulateWsl();
        }

        private async void DistroSetDefault_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.SetDefaultDistribution(SelectedDistro).ConfigureAwait(true);

            PopulateWsl();
        }

        private void DistroShell_Click(object sender, RoutedEventArgs e) => ToolboxClass.ShellDistribution(SelectedDistro);

        private async void DistroStart_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.StartDistribution(SelectedDistro).ConfigureAwait(true);

            PopulateWsl();
        }

        private async void DistroStop_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.TerminateDistribution(SelectedDistro).ConfigureAwait(true);

            PopulateWsl();
        }

        private async void DistroUninstall_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult uninstallMessagebox = MessageBox.Show(
                $"Are you sure you want to uninstall {SelectedDistro.Name}? This will also destroy all data within the distribution.", "Uninstall?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (uninstallMessagebox == MessageBoxResult.Yes)
            {
                _ = await ToolboxClass.UnregisterDistribution(SelectedDistro).ConfigureAwait(true);

                PopulateWsl();
            }
        }

        private void HandleConfiguration()
        {
            SystemTray.Dispose();

            if (Config.Configuration.EnableSystemTray)
            {
                SystemTray.Show();
            }

            if (Config.Configuration.OutputOnStartup)
            {
                OutputWindow.Show();
            }

            ThemeHandler.Set(Config.Configuration.Style);
        }

        private void PopulateSelectedDistro()
        {
            if (SelectedDistro == null)
            {
                DistroSetDefault.IsEnabled = false;
                DistroStart.IsEnabled = false;
                DistroStop.IsEnabled = false;
                DistroConvert.IsEnabled = false;
                DistroRestart.IsEnabled = false;
                DistroUninstall.IsEnabled = false;
                DistroShell.IsEnabled = false;
                DistroExport.IsEnabled = false;

                return;
            }

            DistroStart.IsEnabled = SelectedDistro.State != DistributionClass.StateRunning;
            DistroStop.IsEnabled = SelectedDistro.State == DistributionClass.StateRunning;
            DistroRestart.IsEnabled = true;
            DistroConvert.IsEnabled = SelectedDistro.Version != 2;
            DistroUninstall.IsEnabled = true;
            DistroSetDefault.IsEnabled = !SelectedDistro.IsDefault;
            DistroShell.IsEnabled = SelectedDistro.State == DistributionClass.StateRunning;
            DistroExport.IsEnabled = true;
        }

        private async void PopulateWsl()
        {
            OutputWindow.WriteOutput("Populating...");
            List<DistributionClass> DistroList = await ToolboxClass.ListDistributions().ConfigureAwait(true);

            if (Config.Configuration.HideDockerDistributions)
            {
                _ = DistroList.RemoveAll(distro => distro.Name == "docker-desktop");
                _ = DistroList.RemoveAll(distro => distro.Name == "docker-desktop-data");
            }

            OutputWindow.WriteOutput($"Populated {DistroList.Count} distributions.");
            DistroDetails.ItemsSource = DistroList.FindAll(x => x.IsInstalled);
            DefaultDistribution.Content = ToolboxClass.DefaultDistribution().Name;
        }

        private void RefreshWsl_Click(object sender, RoutedEventArgs e) => PopulateWsl();

        private async void RestartWsl_Click(object sender, RoutedEventArgs e)
        {
            await ToolboxClass.StopWsl().ConfigureAwait(true);
            await ToolboxClass.StartWsl().ConfigureAwait(true);
        }

        private async void StartWsl_Click(object sender, RoutedEventArgs e) => await ToolboxClass.StartWsl().ConfigureAwait(true);

        private async void StatusWsl_Click(object sender, RoutedEventArgs e)
        {
            _ = await ToolboxClass.StatusWsl().ConfigureAwait(true);

            PopulateWsl();
        }

        private async void StopWsl_Click(object sender, RoutedEventArgs e) => await ToolboxClass.StopWsl().ConfigureAwait(true);

        private void ToolboxOutput_Click(object sender, RoutedEventArgs e) => OutputWindow.Show();

        private void ToolboxSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new(Config.Configuration, Config);
            settingsWindow.ShowDialog();

            if ((bool)settingsWindow.DialogResult)
            {
                Config.Save();
                PopulateWsl();
                HandleConfiguration();
            }
        }

        private async void UpdateWsl_Click(object sender, RoutedEventArgs e)
        {
            OutputWindow.Show();
            OutputWindow.WriteOutput("Checking for WSL Updates...");
            CommandClass command = await ToolboxClass.UpdateWsl().ConfigureAwait(true);
            string output = Regex.Replace(command.Output, "\t", " ");
            OutputWindow.WriteOutput(output);
        }
    }
}