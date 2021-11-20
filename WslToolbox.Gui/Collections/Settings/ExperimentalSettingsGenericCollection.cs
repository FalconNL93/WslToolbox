using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class ExperimentalSettingsGenericCollection : GenericCollection
    {
        private readonly SettingsViewModel _viewModel;

        public ExperimentalSettingsGenericCollection(object source) : base(source)
        {
            _viewModel = (SettingsViewModel) source;
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Some functionality may or may not work as expected."
                },
                ElementHelper.ItemExpander("Service polling", ServicePollingControls()),
                ElementHelper.ItemExpander("Shell backend", ShellBackendControls(), controlsEnabled: false)
            };
        }

        private CompositeCollection ServicePollingControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.EnableServicePolling),
                    "Enable service polling",
                    "Configuration.EnableServicePolling",
                    Source),
                ElementHelper.AddNumberBox(nameof(DefaultConfiguration.ServicePollingInterval),
                    "Interval",
                    _viewModel.Configuration.ServicePollingInterval / 1000,
                    "Configuration.ServicePollingInterval",
                    Source,
                    "Configuration.EnableServicePolling")
            };
        }

        private CompositeCollection ShellBackendControls()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Normal,
                    Content = "Only installed backends will be shown in the dropdown menu"
                },
                ElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.ExperimentalConfiguration.ShellBackend),
                    ExperimentalConfiguration.ShellBackendValues(),
                    "Configuration.ExperimentalConfiguration.ShellBackend",
                    Source)
            };
        }
    }
}