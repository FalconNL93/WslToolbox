using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
