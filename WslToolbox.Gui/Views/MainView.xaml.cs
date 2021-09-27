﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using WslToolbox.Core;
using WslToolbox.Gui.Classes;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;
using System.Threading;
using System.ComponentModel;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        private readonly OutputView OutputWindow = new();
        public readonly SystemTrayClass SystemTray = new();
        private readonly AssemblyName GuiAssembly = Assembly.GetExecutingAssembly().GetName();
        private readonly AssemblyName CoreAssembly = GenericClass.Assembly().GetName();
        private MainViewModel ViewModel;
        private DistributionClass SelectedDistro { get; set; }

        public MainView()
        {
            InitializeComponent();
            InitializeViewModel();
            PopulateWsl();
            PopulateSelectedDistro();
            HandleConfiguration();
        }

        private void InitializeViewModel()
        {
            MainViewModel viewModel = new(this);

            DataContext = viewModel;
            ViewModel = viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SystemTray.Dispose();

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void DistroConvert_Click(object sender, RoutedEventArgs e)
        {
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
                CommandClass command = await ToolboxClass.ExportDistribution(SelectedDistro, fileName).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
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

            ImportView importDistroWindow = new(fileName);
            importDistroWindow.ShowDialog();

            if (!(bool)importDistroWindow.DialogResult)
            {
                return;
            }

            try
            {
                CommandClass command = await ToolboxClass.ImportDistribution(SelectedDistro, importDistroWindow.DistroName, importDistroWindow.DistroSelectedDirectory, fileName).ConfigureAwait(true);

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

        public void HandleConfiguration()
        {
            HandleSystemTray();

            if (ViewModel.Config.Configuration.OutputOnStartup)
            {
                OutputWindow.Show();
            }

            ThemeHandler.Set(ViewModel.Config.Configuration.SelectedStyle);
        }

        private void HandleSystemTray()
        {
            SystemTray.Dispose();

            if (ViewModel.Config.Configuration.EnableSystemTray)
            {
                if (ViewModel.Config.Configuration.MinimizeOnStartup)
                {
                    WindowState = WindowState.Minimized;
                    Hide();
                }

                SystemTray.Show();
                SystemTray.Tray.TrayMouseDoubleClick += (sender, args) => ViewModel.ShowApplicationCommand.Execute(null);

                SystemTray.Tray.ContextMenu = new()
                {
                    ItemsSource = ViewModel.SystemTrayMenuItems()
                };
            }
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
            List<DistributionClass> DistroList = await ToolboxClass.
                ListDistributions(ViewModel.Config.Configuration.HideDockerDistributions).
                ConfigureAwait(true);

            DistroDetails.ItemsSource = DistroList.FindAll(x => x.IsInstalled);
            DefaultDistribution.Content = ToolboxClass.DefaultDistribution().Name;
            DistroDetails.ContextMenu = new()
            {
                ItemsSource = ViewModel.DataGridMenuItems()
            };
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

        private async void UpdateWsl_Click(object sender, RoutedEventArgs e)
        {
            UpdateWsl.IsEnabled = false;
            CommandClass command = await ToolboxClass.UpdateWsl().ConfigureAwait(true);
            string output = Regex.Replace(command.Output, "\t", " ");

            if (output.Contains("No updates are available."))
            {
                _ = await this.ShowMessageAsync("WSL Update", "No updates are available.");
            }

            UpdateWsl.IsEnabled = true;
        }

        private async void ToolboxInfo_Click(object sender, RoutedEventArgs e)
        {
            string GuiVersion = $"{GuiAssembly.Version.Major}.{GuiAssembly.Version.Minor}.{GuiAssembly.Version.Build}";
            string CoreVersion = $"{CoreAssembly.Version.Major}.{CoreAssembly.Version.Minor}.{CoreAssembly.Version.Build}";

            _ = await this.ShowMessageAsync("About", $"Gui: {GuiVersion}\nCore: {CoreVersion}");
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (ViewModel.Config.Configuration.MinimizeToTray && ViewModel.Config.Configuration.EnableSystemTray)
            {
                ShowInTaskbar = WindowState != WindowState.Minimized;
            }
        }
    }
}