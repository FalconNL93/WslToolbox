using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ModernWpf.Controls.Primitives;
using WslToolbox.Core;

namespace WslToolbox.Gui.Collections.Dialogs
{
    public static class InstallDistributionDialogCollection
    {
        public static IEnumerable<Control> Items(List<DistributionClass> distributionClass)
        {
            Control[] items =
            {
                DistributionListControl(distributionClass),
                new Label
                {
                    Content = "Note:",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 15, 0, 0)
                },
                new Label
                {
                    Content =
                        "Due to restrictions of WSL, you cannot install an already installed distribution. \n" +
                        "Therefore installed distributions are excluded from the above dropdown menu.\n\n" +
                        "A quick bypass would be to export an already installed distribution and import it \nwith a different name.\n"
                }
            };

            return items;
        }

        private static ComboBox DistributionListControl(List<DistributionClass> distributionClass)
        {
            var installDistributionList = new ComboBox
            {
                Name = "InstallDistributionList",
                ItemsSource = distributionClass.FindAll(x => !x.IsInstalled),
                DisplayMemberPath = "Name",
                MinWidth = 200
            };

            ControlHelper.SetHeader(installDistributionList, "Select an online distribution to install");
            ControlHelper.SetPlaceholderText(installDistributionList, "Select distribution");

            return installDistributionList;
        }
    }
}