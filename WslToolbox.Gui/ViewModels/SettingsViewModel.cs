using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WslToolbox.Gui.Commands;
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
        public DefaultConfiguration Configuration { get; }
        
        
        public ICommand StartOnBoootCommand =>
            new RelayCommand(StartOnBoot, o => true);

        public SettingsViewModel(SettingsView view, DefaultConfiguration configuration, ConfigurationHandler configHandler)
        {
            _view = view;
            ConfigHandler = configHandler;
            Configuration = configuration;
        }

        private void StartOnBoot(object parameter)
        {
            if (StartOnBootHandler.IsEnabled)
            {
                StartOnBootHandler.Disable();
            }
            else
            {
                StartOnBootHandler.Enable();
            }
        }
    }
}