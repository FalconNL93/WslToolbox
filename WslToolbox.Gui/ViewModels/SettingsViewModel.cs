using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Collections.Settings;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers.Ui;
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

            InitializeSettingsTabsElements();
            InitializeSettingsElement();
        }

        public ConfigurationHandler ConfigHandler { get; }
        public DefaultConfiguration Configuration { get; }
        public OsHandler OsHandler { get; }

        public StartOnBootHandler StartOnBootHandler { get; } = new();
        public CompositeCollection GeneralSettings { get; set; }
        public CompositeCollection UpdateSettings { get; set; }
        public CompositeCollection GridSettings { get; set; }
        public CompositeCollection AppearanceSettings { get; set; }
        public CompositeCollection NotificationSettings { get; set; }
        public CompositeCollection ExperimentalSettings { get; set; }
        public CompositeCollection OtherSettings { get; set; }

        private void InitializeSettingsTabsElements()
        {
            GeneralSettings = new GeneralSettingsGenericCollection(this).Items();
            UpdateSettings = new UpdateSettingsGenericCollection(this).Items();
            GridSettings = new GridSettingsGenericCollection(this).Items();
            AppearanceSettings = new AppearanceSettingsGenericCollection(this).Items();
            NotificationSettings = new NotificationSettingsGenericCollection(this).Items();
            ExperimentalSettings = new ExperimentalSettingsGenericCollection(this).Items();
            OtherSettings = new OtherSettingsGenericCollection(this).Items();
        }

        private void InitializeSettingsElement()
        {
            _view.SettingsControl.ItemsSource = new[]
            {
                AddTabItem("General", "GeneralSettings"),
                AddTabItem("Update", "UpdateSettings", enabled: UpdateHandler.IsAvailable()),
                AddTabItem("Grid", "GridSettings"),
                AddTabItem("Appearance", "AppearanceSettings"),
                AddTabItem("Notifications", "NotificationSettings"),
                AddTabItem("Experimental", "ExperimentalSettings",
                    Configuration.ExperimentalConfiguration.ShowExperimentalSettings),
                AddTabItem("Other", "OtherSettings")
            };
        }

        private TabItem AddTabItem(string header, string bind, bool visible = true, bool enabled = true)
        {
            var scrollViewer = new ScrollViewer
            {
                Content = new StackPanel
                {
                    Children = {ElementHelper.AddItemsControl(bind, this)}
                }
            };

            return new TabItem
            {
                Visibility = visible ? Visibility.Visible : Visibility.Collapsed,
                Header = header,
                IsEnabled = enabled,
                Content = scrollViewer
            };
        }
    }
}