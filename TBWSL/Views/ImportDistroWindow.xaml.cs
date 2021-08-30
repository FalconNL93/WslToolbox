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
        private static readonly Regex ValidCharacters = new("^[a-zA-Z0-9]*$");

        public ImportDistroWindow(string path)
        {
            InitializeComponent();
        }

        public string DistroName { get; set; }
        public string DistroSelectedDirectory { get; set; }

        private static bool ValidateDistroName(string DistroName) => ValidCharacters.IsMatch(DistroName) && DistroName.Length >= 3;

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
    }
}