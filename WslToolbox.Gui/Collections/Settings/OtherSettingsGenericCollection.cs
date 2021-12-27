using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ModernWpf.Controls;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class OtherSettingsGenericCollection : GenericCollection
    {
        private readonly SettingsViewModel _viewModel;

        public OtherSettingsGenericCollection(object source) : base(source)
        {
            _viewModel = (SettingsViewModel) Source;
        }

        private StackPanel ConfigurationItems()
        {
            var openConfiguration = new Button
                {Content = "Open file", MinWidth = 95, Margin = new Thickness(0, 0, 5, 10)};
            var resetConfiguration = new Button
                {Content = "Reset configuration", MinWidth = 95, Margin = new Thickness(0, 0, 5, 10)};

            openConfiguration.Click += (_, _) => { ShellHelper.OpenLocal(_viewModel.Configuration.ConfigurationFile); };

            resetConfiguration.Click += OnResetConfiguration;

            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    openConfiguration,
                    resetConfiguration
                }
            };
        }

        private async void OnResetConfiguration(object o, RoutedEventArgs routedEventArgs)
        {
            var resetSettings = DialogHelper.MessageBox("Reset configuration",
                "Do you want to reset your configuration?", "Reset", closeButtonText: "Cancel");

            var resetSettingsResult = await resetSettings.ShowAsync();

            if (resetSettingsResult != ContentDialogResult.Primary) return;
            _viewModel.ConfigHandler.Reset();
            _viewModel.SaveConfigurationAndClose();
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Configuration path"
                },
                ElementHelper.TextBox(nameof(DefaultConfiguration.ConfigurationFile), "Config",
                    "Configuration.ConfigurationFile",
                    Source,
                    bindingMode: BindingMode.OneWay,
                    width: 400,
                    isEnabled: true,
                    isReadonly: true
                ),
                ConfigurationItems(),
                ElementHelper.Separator(),
                ElementHelper.Expander("Advanced", AdvancedControls())
            };
        }

        private CompositeCollection AdvancedControls()
        {
            return new CompositeCollection
            {
                new Label {Content = "Log level"},
                ElementHelper.ComboBox(
                    "MinimumLogLevel",
                    LogConfiguration.GetValues(),
                    "Configuration.MinimumLogLevel",
                    Source
                ),
                new Label {Margin = new Thickness(0, 5, 0, 0), Content = "Update manifest:"},
                new HyperlinkButton
                {
                    Content = AppConfiguration.AppConfigurationUpdateXml,
                    NavigateUri = new Uri(AppConfiguration.AppConfigurationUpdateXml)
                }
            };
        }
    }
}