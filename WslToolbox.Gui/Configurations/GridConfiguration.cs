using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace WslToolbox.Gui.Configurations
{
    public class GridConfiguration
    {
        private const int GridConfigurationDoNothing = 0;
        private const int GridConfigurationOpenTerminal = 1;
        private const int GridConfigurationOpenContextMenu = 2;

        public int DoubleClick { get; set; } = GridConfigurationDoNothing;
        public int SingleClick { get; set; } = GridConfigurationDoNothing;
        public int RightSingleClick { get; set; } = GridConfigurationOpenContextMenu;

        private static Dictionary<int, string> DefaultValues()
        {
            return new Dictionary<int, string>
            {
                {GridConfigurationDoNothing, "Do nothing"},
                {GridConfigurationOpenTerminal, "Open terminal"},
                {GridConfigurationOpenContextMenu, "Open context menu"}
            };
        }

        public static Dictionary<int, string> DoubleClickValues()
        {
            var values = DefaultValues();

            values.Remove(GridConfigurationOpenTerminal);
            values.Remove(GridConfigurationOpenContextMenu);

            return values;
        }

        public static Dictionary<int, string> SingleClickValues()
        {
            var values = DefaultValues();

            values.Remove(GridConfigurationOpenTerminal);
            values.Remove(GridConfigurationOpenContextMenu);

            return values;
        }

        public static Dictionary<int, string> RightSingleClickValues()
        {
            var values = DefaultValues();

            values.Remove(GridConfigurationDoNothing);
            values.Remove(GridConfigurationOpenTerminal);

            return values;
        }
    }
}