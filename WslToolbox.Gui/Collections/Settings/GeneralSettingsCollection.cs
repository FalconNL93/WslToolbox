using System.Windows.Data;

namespace WslToolbox.Gui.Collections.Settings
{
    public class GeneralSettingsCollection : Collections
    {
        public GeneralSettingsCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                AddCheckBox("HideDockerDistributions", "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions"),
                AddCheckBox("EnableSystemTray", "Enable system tray", "Configuration.EnableSystemTray")
            };
        }
    }
}