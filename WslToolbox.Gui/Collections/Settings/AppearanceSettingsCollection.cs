using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Collections.Settings
{
    public class AppearanceSettingsCollection : Collections
    {
        public AppearanceSettingsCollection(object source) : base(source)
        {
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Theme"
                },
                UiElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.SelectedStyle),
                    ThemeConfiguration.GetValues(),
                    "Configuration.SelectedStyle",
                    Source)
            };
        }
    }
}