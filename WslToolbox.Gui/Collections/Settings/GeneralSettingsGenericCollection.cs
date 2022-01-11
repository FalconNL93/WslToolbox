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
                ElementHelper.ToggleSwitch(
                    content: "Create folder in base path",
                    bind: nameof(_generalConfiguration.ImportCreateFolder),
                    source: _generalConfiguration,
                    tooltipContent: "Creates a folder in the selected base path"),

                ElementHelper.ToggleSwitch(
                    content: "Start distribution after import",
                    bind: nameof(_generalConfiguration.ImportStartDistribution),
                    source: _generalConfiguration),

                ElementHelper.ToggleSwitch(
                    content: "Launch terminal after import",
                    bind: nameof(_generalConfiguration.ImportStartTerminal),
                    source: _generalConfiguration,
                    requires: nameof(_generalConfiguration.ImportStartDistribution),
                    tabIndex: 1)
            };
        }

        private CompositeCollection MoveControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(
                    content: "Hide move warning",
                    bind: nameof(_generalConfiguration.HideMoveWarning),
                    source: _generalConfiguration),


                ElementHelper.ToggleSwitch(
                    content: "Copy on move",
                    bind: nameof(_generalConfiguration.CopyOnMove),
                    source: _generalConfiguration,
                    enabled: false,
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
                    nameof(DefaultConfiguration.AppearanceConfiguration.SelectedStyle),
                    Source)
            };
        }

        private CompositeCollection BehaviourControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(
                    content: "Minimize when pressing close button",
                    bind: nameof(_generalConfiguration.MinimizeOnClose),
                    source: _generalConfiguration),

                ElementHelper.ToggleSwitch(
                    content: "Enable system tray",
                    bind: nameof(_generalConfiguration.EnableSystemTray),
                    source: _generalConfiguration),

                ElementHelper.ItemsControlGroup(EnableSystemTraySettings(),
                    source: _generalConfiguration,
                    requires: nameof(_generalConfiguration.EnableSystemTray),
                    tabIndex: 1)
            };
        }

        private CompositeCollection EnableSystemTraySettings()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(
                    content: "Minimize to tray",
                    bind: nameof(_generalConfiguration.MinimizeToTray),
                    source: _generalConfiguration),

                ElementHelper.ToggleSwitch(
                    content: "Minimize on startup",
                    bind: nameof(_generalConfiguration.MinimizeOnStartup),
                    source: _generalConfiguration)
            };
        }
    }
}