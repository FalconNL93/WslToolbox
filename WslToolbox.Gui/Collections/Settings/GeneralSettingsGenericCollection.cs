using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class GeneralSettingsGenericCollection : GenericCollection
    {
        private readonly SettingsViewModel _viewModel;

        public GeneralSettingsGenericCollection(object source) : base(source)
        {
            _viewModel = (SettingsViewModel) source;
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                ElementHelper.ItemExpander("General", GenericControls(), true),
                ElementHelper.ItemExpander("Behaviour", BehaviourControls(), true)
            };
        }

        private CompositeCollection GenericControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.DisableShortcuts),
                    "Disable keyboard shortcuts",
                    "Configuration.DisableShortcuts",
                    Source)
            };
        }

        private CompositeCollection BehaviourControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled",
                    Source),
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.EnableSystemTray),
                    "Enable system tray",
                    "Configuration.EnableSystemTray",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeToTray),
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    Source,
                    "Configuration.EnableSystemTray"),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnStartup),
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    Source,
                    "Configuration.EnableSystemTray"),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button",
                    "Configuration.MinimizeOnClose",
                    Source)
            };
        }
    }
}