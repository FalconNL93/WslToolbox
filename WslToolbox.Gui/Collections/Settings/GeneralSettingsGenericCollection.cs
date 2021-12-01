using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WslToolbox.Gui.Configurations;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.Helpers;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Collections.Settings
{
    public class GeneralSettingsGenericCollection : GenericCollection
    {
        private readonly SettingsViewModel _viewModel;

        public GeneralSettingsGenericCollection(object source) : base(source)
        {
            _viewModel = (SettingsViewModel) source;
        }

        public CompositeCollection Items()
        {
            return new CompositeCollection
            {
                ElementHelper.ItemExpander("General", GenericControls(), true),
                ElementHelper.ItemExpander("Base Path", DefaultBasePathControls(), true),
                ElementHelper.ItemExpander("Behaviour", BehaviourControls(), true)
            };
        }

        private static string SetDefaultBasePath()
        {
            var defaultBasePathDialog = FileDialogHandler.OpenFolderDialog();

            return defaultBasePathDialog.ShowDialog() == null
                ? null
                : Path.GetDirectoryName(defaultBasePathDialog.FileName);
        }

        private CompositeCollection GenericControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideDockerDistributions),
                    "Hide Docker Distributions",
                    "Configuration.HideDockerDistributions",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.HideExportWarning),
                    "Hide export warning",
                    "Configuration.HideExportWarning",
                    Source)
            };
        }

        private CompositeCollection DefaultBasePathControls()
        {
            var viewModel = (SettingsViewModel) Source;

            var defaultBasePath = ElementHelper.AddTextBox(nameof(DefaultConfiguration.UserBasePath),
                null,
                "Configuration.UserBasePath", Source, width: 400);

            var setDefaultBasePathButton = new Button
                {Content = "Set path", Margin = new Thickness(0, 0, 5, 10), MinWidth = 95};

            var openDefaultBasePathButton = new Button
                {Content = "Open path", Margin = new Thickness(0, 0, 5, 10), MinWidth = 95};

            var resetDefaultBasePathButton = new Button
                {Content = "Reset path", Margin = new Thickness(0, 0, 0, 10), MinWidth = 95};

            setDefaultBasePathButton.Click += (_, _) =>
            {
                viewModel.Configuration.UserBasePath = SetDefaultBasePath();
                defaultBasePath.Text = viewModel.Configuration.UserBasePath;
            };

            resetDefaultBasePathButton.Click += (_, _) =>
            {
                viewModel.Configuration.UserBasePath = null;
                defaultBasePath.Text = viewModel.Configuration.UserBasePath;
            };

            openDefaultBasePathButton.Click += (_, _) => { ExplorerHelper.OpenLocal(defaultBasePath.Text); };

            return new CompositeCollection
            {
                new Label
                {
                    FontWeight = FontWeights.Bold,
                    Content = "Default base path"
                },
                new Label
                {
                    Content = "The default base path will only apply to importing distributions."
                },
                defaultBasePath,
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        setDefaultBasePathButton,
                        openDefaultBasePathButton,
                        resetDefaultBasePathButton
                    }
                }
            };
        }

        private CompositeCollection BehaviourControls()
        {
            return new CompositeCollection
            {
                ElementHelper.AddCheckBox("StartOnBoot",
                    "Launch application on system startup",
                    "StartOnBootHandler.IsEnabled",
                    Source),
                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.EnableSystemTray),
                    "Enable system tray",
                    "Configuration.EnableSystemTray",
                    Source),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeToTray),
                    "Minimize to tray",
                    "Configuration.MinimizeToTray",
                    Source,
                    "Configuration.EnableSystemTray"),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnStartup),
                    "Minimize on startup",
                    "Configuration.MinimizeOnStartup",
                    Source,
                    "Configuration.EnableSystemTray"),

                ElementHelper.AddCheckBox(nameof(DefaultConfiguration.MinimizeOnClose),
                    "Minimize when pressing close button",
                    "Configuration.MinimizeOnClose",
                    Source)
            };
        }
    }
}