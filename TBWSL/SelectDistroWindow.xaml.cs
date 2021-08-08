using System.Collections.Generic;
using System.Windows;

namespace WslToolbox
{
    /// <summary>
    /// Interaction logic for SelectDistroWindow.xaml
    /// </summary>
    public partial class SelectDistroWindow : Window
    {
        public SelectDistroWindow()
        {
            InitializeComponent();
            List<DistributionClass> DistroList = ToolboxClass.ListDistributions();
            AvailableDistros.ItemsSource = DistroList.FindAll(x => !x.isInstalled);
            AvailableDistros.DisplayMemberPath = "Name";
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