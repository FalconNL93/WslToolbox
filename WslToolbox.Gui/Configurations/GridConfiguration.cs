using System.Collections.Generic;

namespace WslToolbox.Gui.Configurations
{
    public class GridConfiguration
    {
        public const int GridConfigurationDoNothing = 0;
        public const int GridConfigurationOpenTerminal = 1;
        public const int GridConfigurationOpenContextMenu = 2;

        private int _doubleClick = GridConfigurationOpenTerminal;
        private int _rightSingleClick = GridConfigurationOpenContextMenu;
        private int _singleClick = GridConfigurationDoNothing;

        public bool BasePath { get; set; } = true;
        public bool Size { get; set; } = true;
        public bool Guid { get; set; }

        public int DoubleClick
        {
            get => _doubleClick;
            set => _doubleClick = DoubleClickValues().ContainsKey(value) ? value : GridConfigurationDoNothing;
        }

        public int SingleClick
        {
            get => _singleClick;
            set => _singleClick = SingleClickValues().ContainsKey(value) ? value : GridConfigurationDoNothing;
        }

        public int RightSingleClick
        {
            get => _rightSingleClick;
            set => _rightSingleClick = RightSingleClickValues().ContainsKey(value) ? value : GridConfigurationDoNothing;
        }

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