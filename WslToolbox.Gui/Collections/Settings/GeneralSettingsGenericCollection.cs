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
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions", "Configuration.HideDockerDistributions", Source, header: null),
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.HideExportWarning),
                    "Hide export warning", "Configuration.HideExportWarning", Source, header: null),
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.AutoCheckUpdates),
                    "Automatically check for updates on startup", "Configuration.AutoCheckUpdates", Source,
                    header: null),
            };
        }

        private CompositeCollection AppearanceControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddComboBox(
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
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button", "Configuration.MinimizeOnClose", Source),
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.EnableSystemTray),
                    "Enable system tray", "Configuration.EnableSystemTray", Source),
                ElementHelper.ItemsControlGroup(EnableSystemTraySettings(), source: Source,
                    requires: "Configuration.EnableSystemTray", tabIndex: 1)
            };
        }

        private CompositeCollection EnableSystemTraySettings()
        {
            return new CompositeCollection
            {
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.MinimizeToTray),
                    "Minimize to tray", "Configuration.MinimizeToTray", Source),
                ElementHelper.AddToggleSwitch(nameof(DefaultConfiguration.MinimizeOnStartup),
                    "Minimize on startup", "Configuration.MinimizeOnStartup", Source),
            };
        }
    }
}