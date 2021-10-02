using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;

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
                new Label()
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Theme"
                },
                AddComboBox("SelectedStyle", ThemeConfiguration.GetValues(), "Configuration.SelectedStyle")
            };
        }
    }
}