using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            var viewModel = (SettingsViewModel) _source;

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
                )
            };
        }
    }
}