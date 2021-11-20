using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Collections.Settings
{
    public class AppearanceSettingsGenericCollection : GenericCollection
    {
        public AppearanceSettingsGenericCollection(object source) : base(source)
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
                ElementHelper.AddComboBox(
                    nameof(DefaultConfiguration.AppearanceConfiguration.SelectedStyle),
                    ThemeConfiguration.GetValues(),
                    "Configuration.AppearanceConfiguration.SelectedStyle",
                    Source)
            };
        }
    }
}