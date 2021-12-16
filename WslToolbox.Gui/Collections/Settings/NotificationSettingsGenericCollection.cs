using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;

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
                ElementHelper.ToggleSwitch(nameof(DefaultConfiguration.NotificationConfiguration.Enabled),
                    "Enable notifications", "Configuration.NotificationConfiguration.Enabled", Source, header: null),
                new Separator(),
                ElementHelper.ItemsControlGroup(NotificationControls(),
                    source: Source,
                    requires: "Configuration.NotificationConfiguration.Enabled")
            };
        }

        private CompositeCollection NotificationControls()
        {
            return new CompositeCollection
            {
                ElementHelper.ToggleSwitch(
                    nameof(DefaultConfiguration.NotificationConfiguration.NewVersionAvailable),
                    "New version available", "Configuration.NotificationConfiguration.NewVersionAvailable", Source,
                    "Configuration.AutoCheckUpdates", header: null)
            };
        }
    }
}