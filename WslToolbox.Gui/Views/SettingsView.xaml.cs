using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Serilog.Events;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsView(DefaultConfiguration configuration, ConfigurationHandler configHandler)
        {
            InitializeComponent();
            SettingsViewModel viewModel = new(this, configuration, configHandler);

            DataContext = viewModel;
            _viewModel = viewModel;

            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            OpenJsonFileLink.RequestNavigate += (sender, e) =>
            {
                try
                {
                    if (_viewModel.ConfigHandler.ConfigurationExists)
                        _ = Process.Start(new ProcessStartInfo("explorer")
                        {
                            Arguments = e.Uri.ToString()
                        });
                }
                catch (Exception ex)
                {
                    LogHandler.Log().Error(ex, ex.Message);
                    MessageBox.Show("An error has occurred while executing the request action." +
                                    $"{Environment.NewLine}{Environment.NewLine}{ex.Message}{Environment.NewLine}{Environment.NewLine}" +
                                    "Open log file for more information.", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OpenConfiguration_Click(object sender, RoutedEventArgs e)
        {
            var resetSettings = await this.ShowMessageAsync("Reset configuration",
                "Do you want to reset your configuration?", MessageDialogStyle.AffirmativeAndNegative);

            if (resetSettings != MessageDialogResult.Affirmative) return;
            _viewModel.ConfigHandler.Reset();
            SaveConfigurationAndClose();
        }

        private void SaveConfigurationAndClose()
        {
            DialogResult = true;
            Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedStyle.ItemsSource =
                Enum.GetValues(typeof(ThemeConfiguration.Styles)).Cast<ThemeConfiguration.Styles>();
            MinimumLevel.ItemsSource = Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>();
        }

        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfigurationAndClose();
        }
    }
}