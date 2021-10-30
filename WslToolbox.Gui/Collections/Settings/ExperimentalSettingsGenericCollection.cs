using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class ExperimentalSettingsGenericCollection : GenericCollection
    {
        public ExperimentalSettingsGenericCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            var viewModel = (SettingsViewModel) Source;

            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Some functionality may or may not work as expected."
                },
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.EnableServicePolling),
                    "Service polling",
                    "Configuration.EnableServicePolling",
                    Source),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Interval in seconds"
                },
                UiElementHelper.AddTextBox(nameof(DefaultConfiguration.ServicePollingInterval),
                    $"{viewModel.Configuration.ServicePollingInterval / 1000}",
                    "Configuration.ServicePollingInterval",
                    Source,
                    enabled: false)
            };
        }
    }
}