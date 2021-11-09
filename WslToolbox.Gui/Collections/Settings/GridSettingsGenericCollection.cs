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
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.GridConfiguration.HideWslVersion),
                    "Hide WSL version in Grid",
                    "Configuration.GridConfiguration.HideWslVersion",
                    Source, enabled: false),
                UiElementHelper.ItemExpander("Mouse behaviour", MouseBehaviourControls(), true)
            };
        }

        private CompositeCollection MouseBehaviourControls()
        {
            return new CompositeCollection
            {
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