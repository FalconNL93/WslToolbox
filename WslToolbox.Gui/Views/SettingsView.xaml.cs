using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        private readonly DefaultConfiguration Configuration;
        private readonly ConfigurationHandler ConfigHandler;

        public SettingsView(DefaultConfiguration configuration, ConfigurationHandler configHandler)
        {
            Configuration = configuration;
            DataContext = configuration;
            ConfigHandler = configHandler;

            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void OpenConfiguration_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogResult resetSettings = await this.ShowMessageAsync("Reset configuration", "Do you want to reset your configuration?", MessageDialogStyle.AffirmativeAndNegative);

            if (resetSettings == MessageDialogResult.Affirmative)
            {
                ConfigHandler.Reset();
                SaveConfigurationAndClose();
            }
        }

        private void SaveConfigurationAndClose()
        {
            DialogResult = true;
            Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style.ItemsSource = Enum.GetValues(typeof(ThemeConfiguration.Styles)).Cast<ThemeConfiguration.Styles>();
        }

        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfigurationAndClose();
        }
    }
}