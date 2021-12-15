using System.Windows;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        public SettingsView(DefaultConfiguration configuration, ConfigurationHandler configHandler,
            KeyboardShortcutHandler keyboardShortcutHandler)
        {
            InitializeComponent();
            SettingsViewModel viewModel = new(this, configuration, configHandler, keyboardShortcutHandler);

            DataContext = viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void SaveConfigurationAndClose()
        {
            DialogResult = true;
            Close();
        }

        private void SaveConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfigurationAndClose();
        }
    }
}