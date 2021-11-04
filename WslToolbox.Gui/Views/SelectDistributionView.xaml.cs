using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WslToolbox.Core;

namespace WslToolbox.Gui.Views
{
    /// <summary>
    ///     Interaction logic for SelectDistributionView.xaml
    /// </summary>
    public partial class SelectDistributionView : Window
    {
        public SelectDistributionView()
        {
            InitializeComponent();
            InstallDistribution.IsEnabled = false;
            NoticeBlock.Text = Properties.Resources.NOTICE_INSTALL_EXISTING_DISTRIBUTION;
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