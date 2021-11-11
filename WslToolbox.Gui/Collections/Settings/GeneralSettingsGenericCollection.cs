using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Commands;
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
                UiElementHelper.ItemExpander("Updates", UpdateControls()),
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
                    Source),

                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.DisableShortcuts),
                    "Disable all shortcuts",
                    "Configuration.DisableShortcuts",
                    Source)
            };
        }

        private CompositeCollection UpdateControls()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.AutoCheckUpdates),
                    "Automatically check for updates",
                    "Configuration.AutoCheckUpdates",
                    Source),
                UiElementHelper.AddButton("CheckForUpdates",
                    "Check for updates",
                    command: new CheckForUpdateCommand()
                ),
                new Label
                {
                    Content = $"Current version: {AssemblyHelper.AssemblyVersionHuman}"
                }
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