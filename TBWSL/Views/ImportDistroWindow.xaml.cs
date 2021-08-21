using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace WslToolbox.Views
{
    /// <summary>
    /// Interaction logic for ImportDistroWindow.xaml
    /// </summary>
    public partial class ImportDistroWindow : Window
    {
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

            if(!(bool)openLocation.ShowDialog())
            {
                return;
            }

            DistroSelectedDirectory = Path.GetDirectoryName(openLocation.FileName);
            ImportDistroLocation.Text = Path.GetDirectoryName(openLocation.FileName);

            DistroName = ImportDistroName.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
