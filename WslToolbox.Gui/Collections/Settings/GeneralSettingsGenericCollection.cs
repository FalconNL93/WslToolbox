using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
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
                UiElementHelper.ItemExpander("General", GenericControls(), true),
                UiElementHelper.ItemExpander("Behaviour", BehaviourControls())
            };
        }

        private CompositeCollection GenericControls()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions",
                    Source),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.DisableDeleteCommand),
                    "Disable the delete distribution menu item",
                    "Configuration.DisableDeleteCommand",
                    Source)
            };
        }

        private CompositeCollection BehaviourControls()
        {
            return new CompositeCollection
            {
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
                    "Configuration.EnableSystemTray"),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button",
                    "Configuration.MinimizeOnClose",
                    Source)
            };
        }
    }
}