using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Collections.Settings
{
    public class GeneralSettingsGenericCollection : GenericCollection
    {
        public GeneralSettingsGenericCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions",
                    Source),

                UiElementHelper.AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled",
                    Source),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.EnableSystemTray),
                    "Enable system tray",
                    "Configuration.EnableSystemTray",
                    Source),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeToTray),
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    Source,
                    "Configuration.EnableSystemTray"),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnStartup),
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    Source,
                    "Configuration.EnableSystemTray")
            };
        }
    }
}