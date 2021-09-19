using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using WslToolbox.Core;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    /// Interaction logic for SelectDistroWindow.xaml
    /// </summary>
    public partial class SelectDistroWindow : MetroWindow
    {
        public SelectDistroWindow()
        {
            InitializeComponent();
            InstallDistro.IsEnabled = false;
            NoticeBlock.Text = "Due to current restrictions in WSL CLI, installing an existing distro is not possible. You can export an existing distro and import it back with a different name.";
            AvailableDistros.IsEnabled = false;

            List<DistributionClass> distroList = FetchDistributions();
            AvailableDistros.ItemsSource = distroList.FindAll(x => !x.IsInstalled);
            AvailableDistros.DisplayMemberPath = "Name";
            AvailableDistros.IsEnabled = true;
        }

        private static List<DistributionClass> FetchDistributions()
        {
            Task<List<DistributionClass>> distroList = Task.Run(async () => await ToolboxClass.ListDistributions().ConfigureAwait(true));

            return distroList.Result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InstallDistro_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableDistros.SelectedItem != null)
            {
                DialogResult = true;
            }

            Close();
        }

        private void AvailableDistros_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InstallDistro.IsEnabled = true;
        }
    }
}