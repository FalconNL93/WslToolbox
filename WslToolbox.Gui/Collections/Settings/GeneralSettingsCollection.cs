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
                AddCheckBox("HideDockerDistributions",
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions"),

                AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled"),

                AddCheckBox("EnableSystemTray",
                    "Enable system tray",
                    "Configuration.EnableSystemTray"),

                AddCheckBox("MinimizeToTray",
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    "Configuration.EnableSystemTray"),

                AddCheckBox("MinimizeOnStartup",
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    "Configuration.EnableSystemTray")
            };
        }
    }
}