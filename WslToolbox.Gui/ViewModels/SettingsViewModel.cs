using System.Windows.Data;
using WslToolbox.Gui.Collections.Settings;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class SettingsViewModel
    {
        private readonly SettingsView _view;

        public SettingsViewModel(SettingsView view, DefaultConfiguration configuration,
            ConfigurationHandler configHandler)
        {
            _view = view;
            ConfigHandler = configHandler;
            Configuration = configuration;
            GeneralSettings = new GeneralSettingsCollection(this).Items();
        }

        public StartOnBootHandler StartOnBootHandler { get; } = new();
        public ConfigurationHandler ConfigHandler { get; }
        public DefaultConfiguration Configuration { get; }

        public CompositeCollection GeneralSettings { get; }
    }
}