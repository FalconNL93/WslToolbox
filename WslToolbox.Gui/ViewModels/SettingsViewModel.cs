using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class SettingsViewModel
    {
        private readonly SettingsView _view;
        public StartOnBootHandler StartOnBootHandler { get; } = new();
        public ConfigurationHandler ConfigHandler { get; }
        private DefaultConfiguration Configuration { get; }

        public SettingsViewModel(SettingsView view, DefaultConfiguration configuration,
            ConfigurationHandler configHandler)
        {
            _view = view;
            ConfigHandler = configHandler;
            Configuration = configuration;
        }
    }
}