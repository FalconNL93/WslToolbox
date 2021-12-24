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
                ElementHelper.ItemsControlGroup(ImportControls(), header: "Import"),
                ElementHelper.Separator(),
                ElementHelper.ItemsControlGroup(MoveControls(), header: "Move"),
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

        private CompositeCollection ImportControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.ImportCreateFolder),
                    "Create folder in base path", "Configuration.ImportCreateFolder", Source, header: null,
                    tooltipContent: "Creates a folder in the selected base path"),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.ImportStartDistribution),
                    "Start distribution after import", "Configuration.ImportStartDistribution", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.ImportStartTerminal),
                    "Launch terminal after import", "Configuration.ImportStartTerminal", Source, header: null,
                    requires: "Configuration.ImportStartDistribution", tabIndex: 1)
            };
        }

        private CompositeCollection MoveControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.HideMoveWarning),
                    "Hide move warning", "Configuration.HideMoveWarning", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.CopyOnMove),
                    "Copy on move", "Configuration.CopyOnMove", Source, header: null, enabled: false,
                    tooltipContent:
                    "Copies the contents of the distribution and deletes the source after completion. More safe than regular move action.")
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