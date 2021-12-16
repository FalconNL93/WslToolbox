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
        public readonly KeyboardShortcutHandler KeyboardShortcutHandler;

        public SettingsViewModel(SettingsView view, DefaultConfiguration configuration,
            ConfigurationHandler configHandler, KeyboardShortcutHandler keyboardShortcutHandler)
        {
            _view = view;
            ConfigHandler = configHandler;
            Configuration = configuration;
            KeyboardShortcutHandler = keyboardShortcutHandler;

            InitializeSettingsTabsElements();
            InitializeSettingsElement();
        }

        public ConfigurationHandler ConfigHandler { get; }
        public DefaultConfiguration Configuration { get; }
        public CompositeCollection GeneralSettings { get; set; }
        public CompositeCollection KeyboardShortcutSettings { get; set; }
        public CompositeCollection GridSettings { get; set; }
        public CompositeCollection NotificationSettings { get; set; }
        public CompositeCollection OtherSettings { get; set; }

        private void InitializeSettingsTabsElements()
        {
            GeneralSettings = new GeneralSettingsGenericCollection(this).Items();
            KeyboardShortcutSettings = new KeyboardShortcutSettingsGenericCollection(this).Items();
            GridSettings = new GridSettingsGenericCollection(this).Items();
            NotificationSettings = new NotificationSettingsGenericCollection(this).Items();
            OtherSettings = new OtherSettingsGenericCollection(this).Items();
        }

        public void SaveConfigurationAndClose()
        {
            _view.SaveConfigurationAndClose();
        }

        private void InitializeSettingsElement()
        {
            _view.SettingsControl.ItemsSource = new[]
            {
                AddTabItem("General", "GeneralSettings"),
                AddTabItem("Shortcuts", "KeyboardShortcutSettings"),
                AddTabItem("Grid", "GridSettings"),
                AddTabItem("Notifications", "NotificationSettings"),
                AddTabItem("Other", "OtherSettings")
            };
        }

        private TabItem AddTabItem(string header, string bind, bool visible = true, bool enabled = true)
        {
            var scrollViewer = new ScrollViewer
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new Grid
                        {
                            Margin = new Thickness(0, 0, 15, 0),
                            Children = {ElementHelper.ItemsControl(bind, this)}
                        }
                    }
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