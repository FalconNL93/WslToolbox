using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WslToolbox.Core;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public static class InstallDistributionDialogCollection
    {
        public static IEnumerable<Control> Items(List<DistributionClass> distributionClass)
        {
            Control[] items =
            {
                new Label
                {
                    Content = "Select an online distribution to install",
                    Margin = new Thickness(0, 0, 0, 5),
                },
                new ComboBox
                {
                    Name = "InstallDistributionList",
                    ItemsSource = distributionClass.FindAll(x => !x.IsInstalled),
                    DisplayMemberPath = "Name",
                    SelectedIndex = 0,
                    MinWidth = 200
                }
            };

            return items;
        }
    }
}