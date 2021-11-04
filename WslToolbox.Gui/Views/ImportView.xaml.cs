using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        private static readonly Regex ValidCharacters = new("^[a-zA-Z0-9]*$");

        public ImportView(string path)
        {
            InitializeComponent();
        }

        public string DistributionName { get; set; }
        public string DistributionSelectedDirectory { get; set; }

        private static bool ValidateDistributionName(string distributionName)
        {
            return ValidCharacters.IsMatch(distributionName) && distributionName.Length >= 3;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateDistributionName(ImportDistributionName.Text))
            {
                _ = MessageBox.Show("Only alphanumeric characters are allowed, minimum characters are 3.",
                    "Import distribution", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(ImportDistributionLocation.Text) ||
                ImportDistributionLocation.Text == "Double click here to browse...")
            {
                _ = MessageBox.Show("Installation location does not exist or is not selected.", "Import distribution",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void ImportDistributionLocation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openLocation = new()
            {
                Title = "Export",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select folder",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (!(bool) openLocation.ShowDialog()) return;

            DistributionSelectedDirectory = Path.GetDirectoryName(openLocation.FileName);
            ImportDistributionLocation.Text = Path.GetDirectoryName(openLocation.FileName);

            DistributionName = ImportDistributionName.Text;
        }

        private void ImportDistributionName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ImportDistributionButton.IsEnabled = ImportDistributionName.Text.Length >= 1;
        }
    }
}