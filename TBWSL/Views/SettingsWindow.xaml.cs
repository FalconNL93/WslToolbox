using System.Windows;
using WslToolbox.Configurations;

namespace WslToolbox.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private DefaultConfiguration configuration;

        public SettingsWindow(DefaultConfiguration configuration)
        {
            InitializeComponent();
            this.configuration = configuration;

            HideDockerDistributions.IsChecked = configuration.HideDockerDistributions;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveConfiguration_Click(object sender, RoutedEventArgs e)
        {
            configuration.HideDockerDistributions = (bool)HideDockerDistributions.IsChecked;
            Close();
        }
    }
}