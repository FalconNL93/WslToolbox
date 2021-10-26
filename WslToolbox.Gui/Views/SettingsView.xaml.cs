using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

        public SettingsView(DefaultConfiguration configuration, ConfigurationHandler configHandler, OsHandler osHandler)
        {
            InitializeComponent();
            SettingsViewModel viewModel = new(this, configuration, configHandler, osHandler);

            DataContext = viewModel;
            _viewModel = viewModel;
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
        }

        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfigurationAndClose();
        }
    }
}