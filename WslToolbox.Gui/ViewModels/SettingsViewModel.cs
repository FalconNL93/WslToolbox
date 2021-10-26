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
            ConfigurationHandler configHandler, OsHandler osHandler)
        {
            _view = view;
            ConfigHandler = configHandler;
            Configuration = configuration;
            OsHandler = osHandler;

            InitializeSettingsElements();
        }

        public ConfigurationHandler ConfigHandler { get; }
        public DefaultConfiguration Configuration { get; }
        public OsHandler OsHandler { get; }

        public StartOnBootHandler StartOnBootHandler { get; } = new();
        public CompositeCollection GeneralSettings { get; set; }
        public CompositeCollection GridSettings { get; set; }
        public CompositeCollection AppearanceSettings { get; set; }
        public CompositeCollection OtherSettings { get; set; }

        private void InitializeSettingsElements()
        {
            GeneralSettings = new GeneralSettingsGenericCollection(this).Items();
            GridSettings = new GridSettingsGenericCollection(this).Items();
            AppearanceSettings = new AppearanceSettingsGenericCollection(this).Items();
            OtherSettings = new OtherSettingsGenericCollection(this).Items();
        }
    }
}