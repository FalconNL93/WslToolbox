using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WslToolbox.Views
{
    /// <summary>
    /// Interaction logic for ImportDistroWindow.xaml
    /// </summary>
    public partial class ImportDistroWindow : Window
    {
        private static readonly Regex Regex = new Regex("^[a-zA-Z0-9]*$");

        public string DistroSelectedDirectory { get; set; }
        public string DistroName { get; set; }

        public ImportDistroWindow(string path)
        {
            InitializeComponent();
        }

        private void ImportDistroLocation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

            if (!(bool)openLocation.ShowDialog())
            {
                return;
            }

            DistroSelectedDirectory = Path.GetDirectoryName(openLocation.FileName);
            ImportDistroLocation.Text = Path.GetDirectoryName(openLocation.FileName);

            DistroName = ImportDistroName.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateDistroName(ImportDistroName.Text))
            {
                _ = MessageBox.Show("Only alphanumeric characters are allowed, minimum characters are 3.", "Import distribution", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(ImportDistroLocation.Text) || ImportDistroLocation.Text == "Double click here to browse...")
            {
                _ = MessageBox.Show("Installation location does not exist or is not selected.", "Import distribution", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private static bool ValidateDistroName(string DistroName) => Regex.IsMatch(DistroName) && DistroName.Length >= 3;
    }
}