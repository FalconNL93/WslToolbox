using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class OtherSettingsGenericCollection : GenericCollection
    {
        private readonly SettingsViewModel _viewModel;

        public OtherSettingsGenericCollection(object source) : base(source)
        {
            _viewModel = (SettingsViewModel) Source;
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Configuration path (right click to copy):"
                },
                ElementHelper.AddHyperlink(
                    _viewModel.Configuration.ConfigurationFile,
                    tooltip: _viewModel.Configuration.ConfigurationFile,
                    contextMenuItems: GenericMenuCollection.CopyToClipboard(_viewModel.Configuration.ConfigurationFile)
                ),
                ElementHelper.ItemExpander("Advanced", AdvancedControls())
            };
        }


        private CompositeCollection AdvancedControls()
        {
            return new CompositeCollection
            {
                new Label
                {
                    Content = "Log level"
                },
                ElementHelper.AddComboBox(
                    "MinimumLogLevel",
                    LogConfiguration.GetValues(),
                    "Configuration.MinimumLogLevel",
                    Source
                ),
                ElementHelper.AddCheckBox(
                    nameof(DefaultConfiguration.ExperimentalConfiguration.ShowExperimentalSettings),
                    "Show experimental configuration",
                    "Configuration.ExperimentalConfiguration.ShowExperimentalSettings",
                    Source
                )
            };
        }
    }
}