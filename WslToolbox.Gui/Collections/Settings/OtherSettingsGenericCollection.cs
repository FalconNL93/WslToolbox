using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WslToolbox.Core;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class OtherSettingsGenericCollection : GenericCollection
    {
        public OtherSettingsGenericCollection(object source) : base(source)
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
                    Content = "Configuration File"
                },
                UiElementHelper.AddHyperlink(
                    viewModel.Configuration.ConfigurationFile,
                    tooltip: viewModel.Configuration.ConfigurationFile
                ),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Version"
                },
                new TextBlock
                {
                    Padding = new Thickness(5, 0, 0, 10),
                    Inlines =
                    {
                        new Run($"GUI: {AssemblyHelper.AssemblyVersionHuman}"),
                        new Run(Environment.NewLine),
                        new Run($"Core: {GenericClass.AssemblyVersionHuman}")
                    }
                },
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Log level"
                },
                UiElementHelper.AddComboBox(
                    "MinimumLevel",
                    LogConfiguration.GetValues(),
                    "Configuration.Logging.MinimumLevel",
                    Source
                ),
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = OsHandler.Supported()
                        ? "OS is supported"
                        : "OS is not supported"
                },
                UiElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideUnsupportedOsMessage),
                    "Hide unsupported operating system notification",
                    "Configuration.HideUnsupportedOsMessage",
                    Source
                )
            };
        }
    }
}