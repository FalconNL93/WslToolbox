using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Collections.Settings
{
    public class NotificationSettingsGenericCollection : GenericCollection
    {
        public NotificationSettingsGenericCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.NotificationConfiguration.Enabled),
                    "Enable notifications",
                    "Configuration.NotificationConfiguration.Enabled",
                    Source),
                new Separator(),
                UiElementHelper.ItemsControlGroup(NotificationControls(),
                    source: Source,
                    requires: "Configuration.NotificationConfiguration.Enabled")
            };
        }

        private CompositeCollection NotificationControls()
        {
            return new CompositeCollection
            {
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.NotificationConfiguration.NewVersionAvailable),
                    "New version available",
                    "Configuration.NotificationConfiguration.NewVersionAvailable",
                    Source,
                    "Configuration.AutoCheckUpdates"
                )
            };
        }
    }
}