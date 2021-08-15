using System.Windows;
using WslToolbox.Configurations;

namespace WslToolbox.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private DefaultConfiguration Configuration;

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
            Close();
        }
    }
}