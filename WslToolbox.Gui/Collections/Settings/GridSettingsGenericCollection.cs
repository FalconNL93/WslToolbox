using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Collections.Settings
{
    public class GridSettingsGenericCollection : GenericCollection
    {
        public GridSettingsGenericCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Double mouse click should"
                },
                UiElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.DoubleClick),
                    GridConfiguration.DoubleClickValues(),
                    "Configuration.GridConfiguration.DoubleClick",
                    Source),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Single mouse click should"
                },
                UiElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.SingleClick),
                    GridConfiguration.SingleClickValues(),
                    "Configuration.GridConfiguration.SingleClick",
                    Source),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Right mouse click should"
                },
                UiElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.RightSingleClick),
                    GridConfiguration.RightSingleClickValues(),
                    "Configuration.GridConfiguration.RightSingleClick",
                    Source)
            };
        }
    }
}