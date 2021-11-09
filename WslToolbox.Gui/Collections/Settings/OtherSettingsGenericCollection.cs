using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WslToolbox.Core;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
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
                UiElementHelper.ItemExpander("Configuration", ConfigurationControls(), true),
                UiElementHelper.ItemExpander("About", AboutControls(), true),
                UiElementHelper.ItemExpander("Advanced", AdvancedControls())
            };
        }

        private CompositeCollection ConfigurationControls()
        {
            return new CompositeCollection
            {
                new Label
                {
                    Content = "Path (right click to copy):"
                },
                UiElementHelper.AddHyperlink(
                    _viewModel.Configuration.ConfigurationFile,
                    tooltip: _viewModel.Configuration.ConfigurationFile,
                    contextMenuItems: GenericMenuCollection.CopyToClipboard(_viewModel.Configuration.ConfigurationFile)
                )
            };
        }

        private CompositeCollection AboutControls()
        {
            return new CompositeCollection
            {
                new TextBlock
                {
                    Inlines =
                    {
                        new Run($"GUI: {AssemblyHelper.AssemblyVersionHuman}"),
                        new Run(Environment.NewLine),
                        new Run($"Core: {GenericClass.AssemblyVersionHuman}")
                    }
                }
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
                UiElementHelper.AddComboBox(
                    "MinimumLogLevel",
                    LogConfiguration.GetValues(),
                    "Configuration.MinimumLogLevel",
                    Source
                ),
                UiElementHelper.AddCheckBox(
                    nameof(DefaultConfiguration.ExperimentalConfiguration.ShowExperimentalSettings),
                    "Show experimental configuration",
                    "Configuration.ExperimentalConfiguration.ShowExperimentalSettings",
                    Source
                )
            };
        }
    }
}