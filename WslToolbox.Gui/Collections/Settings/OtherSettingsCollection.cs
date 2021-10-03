using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class OtherSettingsCollection : Collections
    {
        public OtherSettingsCollection(object source) : base(source)
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
                }
            };
        }
    }
}