using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public static class InstallDistributionDialogCollection
    {
        public static IEnumerable<Control> Items(MainViewModel viewModel)
        {
            Control[] items =
            {
                new Label
                {
                    Content = "Distribution",
                    Margin = new Thickness(0, 0, 0, 5),
                    FontWeight = FontWeights.Bold
                },
                new ComboBox
                {
                    Name = "InstallDistributionList",
                    ItemsSource = viewModel.DistributionList.FindAll(x => !x.IsInstalled),
                    DisplayMemberPath = "Name",
                    SelectedIndex = 0,
                    MinWidth = 200
                }
            };

            return items;
        }
    }
}