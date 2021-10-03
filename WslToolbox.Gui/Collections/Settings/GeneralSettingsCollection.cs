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
                    _source),

                UiElementHelper.AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled",
                    _source),

                UiElementHelper.AddCheckBox("EnableSystemTray",
                    "Enable system tray",
                    "Configuration.EnableSystemTray",
                    _source),

                UiElementHelper.AddCheckBox("MinimizeToTray",
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    _source,
                    "Configuration.EnableSystemTray"),

                UiElementHelper.AddCheckBox("MinimizeOnStartup",
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    _source,
                    "Configuration.EnableSystemTray")
            };
        }
    }
}