using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Collections.Settings
{
    public class UpdateSettingsGenericCollection : GenericCollection
    {
        public UpdateSettingsGenericCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddItemGroup(UpdateControls(), false, UpdateHandler.IsAvailable())
            };
        }

        private CompositeCollection UpdateControls()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.AutoCheckUpdates),
                    "Automatically check for updates",
                    "Configuration.AutoCheckUpdates",
                    Source
                ),
                new Label
                {
                    Content = $"Current version: {AssemblyHelper.AssemblyVersionHuman}"
                }
            };
        }
    }
}