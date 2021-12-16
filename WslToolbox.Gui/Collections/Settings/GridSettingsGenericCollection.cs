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
                ElementHelper.ItemsControlGroup(ColumnControls(), header: "Columns"),
                ElementHelper.Separator(),
                ElementHelper.ItemsControlGroup(MouseBehaviourControls(), header: "Grid control")
            };
        }

        private CompositeCollection ColumnControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GridConfiguration.BasePath),
                    "Base path", "Configuration.GridConfiguration.BasePath", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GridConfiguration.Size),
                    "Size", "Configuration.GridConfiguration.Size", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GridConfiguration.Guid),
                    "GUID", "Configuration.GridConfiguration.Guid", Source, header: null)
            };
        }

        private CompositeCollection MouseBehaviourControls()
        {
            return new CompositeCollection
            {
                new Label {Content = "Double click should"},
                ElementHelper.ComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.DoubleClick),
                    GridConfiguration.DoubleClickValues(),
                    "Configuration.GridConfiguration.DoubleClick",
                    Source),
                new Label {Content = "Single mouse click should"},
                ElementHelper.ComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.SingleClick),
                    GridConfiguration.SingleClickValues(),
                    "Configuration.GridConfiguration.SingleClick",
                    Source),
                new Label {Content = "Right mouse click should"},
                ElementHelper.ComboBox(
                    nameof(DefaultConfiguration.GridConfiguration.RightSingleClick),
                    GridConfiguration.RightSingleClickValues(),
                    "Configuration.GridConfiguration.RightSingleClick",
                    Source)
            };
        }
    }
}