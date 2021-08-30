using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using WslToolbox.Classes;

namespace WslToolbox.Views
{
    /// <summary>
    /// Interaction logic for SelectDistroWindow.xaml
    /// </summary>
    public partial class SelectDistroWindow : Window
    {
        public SelectDistroWindow()
        {
            InitializeComponent();
            NoticeBlock.Text = "Due to current restrictions in WSL CLI, installing an existing distro is not possible. You can export an existing distro and import it back with a different name.";
            AvailableDistros.IsEnabled = false;

            List<DistributionClass> distroList = FetchDistributions();
            AvailableDistros.ItemsSource = distroList.FindAll(x => !x.IsInstalled);
            AvailableDistros.DisplayMemberPath = "Name";
            AvailableDistros.IsEnabled = true;
        }

        private static List<DistributionClass> FetchDistributions()
        {
            Task<List<DistributionClass>> distroList = Task.Run(async () => await ToolboxClass.ListDistributions().ConfigureAwait(false));

            return distroList.Result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableDistros.SelectedItem != null)
            {
                DialogResult = true;
            }

            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}