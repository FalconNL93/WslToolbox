using System.Windows.Data;
using WslToolbox.Gui.Helpers;

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
                UiElementHelper.AddCheckBox("HideDockerDistributions",
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions",
                    Source),

                UiElementHelper.AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled",
                    Source),

                UiElementHelper.AddCheckBox("EnableSystemTray",
                    "Enable system tray",
                    "Configuration.EnableSystemTray",
                    Source),

                UiElementHelper.AddCheckBox("MinimizeToTray",
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    Source,
                    "Configuration.EnableSystemTray"),

                UiElementHelper.AddCheckBox("MinimizeOnStartup",
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    Source,
                    "Configuration.EnableSystemTray")
            };
        }
    }
}