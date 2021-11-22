using System.Collections.Generic;
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
                    Content = "Select distribution to install"
                },
                new ComboBox
                {
                    Name = "InstallDistributionList",
                    ItemsSource = viewModel.DistributionList.FindAll(x => !x.IsInstalled),
                    DisplayMemberPath = "Name",
                    SelectedIndex = 0
                }
            };

            return items;
        }
    }
}