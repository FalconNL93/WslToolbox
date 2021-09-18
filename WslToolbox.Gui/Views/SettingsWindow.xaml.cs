using System.Windows;
using WslToolbox.Gui.Configurations;
using MahApps.Metro.Controls;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        private readonly DefaultConfiguration Configuration;

        public SettingsWindow(DefaultConfiguration configuration)
        {
            Configuration = configuration;
            DataContext = configuration;

            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveConfiguration_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}