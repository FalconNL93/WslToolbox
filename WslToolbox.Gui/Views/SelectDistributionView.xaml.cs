using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using WslToolbox.Core;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for SelectDistributionView.xaml
    /// </summary>
    public partial class SelectDistributionView : MetroWindow
    {
        public SelectDistributionView()
        {
            InitializeComponent();
            InstallDistribution.IsEnabled = false;
            NoticeBlock.Text =
                "Due to current restrictions in WSL CLI, installing an existing distribution is not possible. You can export an existing distro and import it back with a different name.";
            AvailableDistributions.IsEnabled = false;

            var distributionList = FetchDistributions();
            AvailableDistributions.ItemsSource = distributionList.FindAll(x => !x.IsInstalled);
            AvailableDistributions.DisplayMemberPath = "Name";
            AvailableDistributions.IsEnabled = true;
        }

        private static List<DistributionClass> FetchDistributions()
        {
            var distributionList = Task.Run(async () => await ToolboxClass.ListDistributions().ConfigureAwait(true));

            return distributionList.Result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InstallDistribution_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableDistributions.SelectedItem != null) DialogResult = true;

            Close();
        }

        private void AvailableDistribution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InstallDistribution.IsEnabled = true;
        }
    }
}