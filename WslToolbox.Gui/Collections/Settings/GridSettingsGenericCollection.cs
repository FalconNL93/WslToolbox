using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;

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
                ElementHelper.ItemExpander("Columns", ColumnControls(), true),
                ElementHelper.ItemExpander("Mouse behaviour", MouseBehaviourControls(), true)
            };
        }

        private CompositeCollection ColumnControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.GridConfiguration.BasePath),
                    "Base path",
                    "Configuration.GridConfiguration.BasePath",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.GridConfiguration.Size),
                    "Size",
                    "Configuration.GridConfiguration.Size",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.GridConfiguration.Guid),
                    "Guid",
                    "Configuration.GridConfiguration.Guid",
                    Source)
            };
        }

        private CompositeCollection MouseBehaviourControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.DoubleClick),
                    GridConfiguration.DoubleClickValues(),
                    "Configuration.GridConfiguration.DoubleClick",
                    Source),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Single mouse click should"
                },
                ElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.SingleClick),
                    GridConfiguration.SingleClickValues(),
                    "Configuration.GridConfiguration.SingleClick",
                    Source),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Right mouse click should"
                },
                ElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.RightSingleClick),
                    GridConfiguration.RightSingleClickValues(),
                    "Configuration.GridConfiguration.RightSingleClick",
                    Source)
            };
        }
    }
}