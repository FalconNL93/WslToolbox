using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Configurations.Sections;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class SettingsItem
    {
        public string Content { get; set; }
        public Control Control { get; set; }
    }

    public class GeneralSettingsGenericCollection : GenericCollection
    {
        private readonly GeneralConfiguration _generalConfiguration;

        public GeneralSettingsGenericCollection(object source) : base(source)
        {
            var sourceConfiguration = (SettingsViewModel) source;
            _generalConfiguration = sourceConfiguration.Configuration.GeneralConfiguration;
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
                ElementHelper.ToggleSwitch(
                    content: "Hide Docker Distributions",
                    bind: nameof(_generalConfiguration.HideDockerDistributions),
                    source: _generalConfiguration),

                ElementHelper.ToggleSwitch(
                    content: "Hide export warning",
                    bind: nameof(_generalConfiguration.HideExportWarning),
                    source: _generalConfiguration),

                ElementHelper.ToggleSwitch(
                    content: "Automatically check for updates on startup",
                    bind: nameof(_generalConfiguration.AutoCheckUpdates),
                    source: _generalConfiguration)
            };
        }

        private CompositeCollection ImportControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.ImportCreateFolder),
                    "Create folder in base path", "Configuration.ImportCreateFolder", Source, header: null,
                    tooltipContent: "Creates a folder in the selected base path"),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.ImportStartDistribution),
                    "Start distribution after import", "Configuration.ImportStartDistribution", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.ImportStartTerminal),
                    "Launch terminal after import", "Configuration.ImportStartTerminal", Source, header: null,
                    requires: "Configuration.ImportStartDistribution", tabIndex: 1)
            };
        }

        private CompositeCollection MoveControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.HideMoveWarning),
                    "Hide move warning", "Configuration.HideMoveWarning", Source, header: null),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.CopyOnMove),
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
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button", "Configuration.MinimizeOnClose", Source),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.EnableSystemTray),
                    "Enable system tray", "Configuration.EnableSystemTray", Source),
                ElementHelper.ItemsControlGroup(EnableSystemTraySettings(), source: Source,
                    requires: "Configuration.EnableSystemTray", tabIndex: 1)
            };
        }

        private CompositeCollection EnableSystemTraySettings()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.MinimizeToTray),
                    "Minimize to tray", "Configuration.MinimizeToTray", Source),
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.GeneralConfiguration.MinimizeOnStartup),
                    "Minimize on startup", "Configuration.MinimizeOnStartup", Source)
            };
        }
    }
}