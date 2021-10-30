using System.Text.Json.Serialization;
using Serilog.Events;

namespace WslToolbox.Gui.Configurations
{
    public class AppearanceConfiguration
    {
        public ThemeConfiguration.Styles SelectedStyle { get; set; } = ThemeConfiguration.Styles.Auto;
    }
}