using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;

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
                ElementHelper.ItemsControlGroup(GenericControls()),
                ElementHelper.Separator(),
                ElementHelper.ItemsControlGroup(AppearanceControls(), header: "Theme"),
                ElementHelper.Separator(),
                ElementHelper.ItemsControlGroup(BehaviourControls(), header: "System tray")
            };
        }

        private CompositeCollection GenericControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions", "Configuration.HideDockerDistributions", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.HideExportWarning),
                    "Hide export warning", "Configuration.HideExportWarning", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.AutoCheckUpdates),
                    "Automatically check for updates on startup", "Configuration.AutoCheckUpdates", Source,
                    header: null)
            };
        }

        private CompositeCollection AppearanceControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ComboBox(
                    nameof(DefaultConfiguration.AppearanceConfiguration.SelectedStyle),
                    ThemeConfiguration.GetValues(),
                    "Configuration.AppearanceConfiguration.SelectedStyle",
                    Source)
            };
        }

        private CompositeCollection BehaviourControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button", "Configuration.MinimizeOnClose", Source),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.EnableSystemTray),
                    "Enable system tray", "Configuration.EnableSystemTray", Source),
                ElementHelper.ItemsControlGroup(EnableSystemTraySettings(), source: Source,
                    requires: "Configuration.EnableSystemTray", tabIndex: 1)
            };
        }

        private CompositeCollection EnableSystemTraySettings()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.MinimizeToTray),
                    "Minimize to tray", "Configuration.MinimizeToTray", Source),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.MinimizeOnStartup),
                    "Minimize on startup", "Configuration.MinimizeOnStartup", Source)
            };
        }
    }
}