using AutoUpdaterDotNET;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Handlers
{
    public static class UpdateHandler
    {
        private static void SetConfiguration(UpdateConfiguration updateConfiguration)
        {
            AutoUpdater.ShowSkipButton = updateConfiguration.ShowSkipButton;
            AutoUpdater.ShowRemindLaterButton = updateConfiguration.ShowRemindLaterButton;
            AutoUpdater.AppTitle = updateConfiguration.AppTitle;
            AutoUpdater.Mandatory = updateConfiguration.Mandatory;
            AutoUpdater.UpdateMode = updateConfiguration.UpdateMode;
            AutoUpdater.OpenDownloadPage = updateConfiguration.OpenDownloadPage;
        }

        public static void ShowDialog()
        {
            var configuration = new UpdateConfiguration();
            SetConfiguration(configuration);

            AutoUpdater.Start(configuration.Url);
            AutoUpdater.CheckForUpdateEvent += args =>
            {
                if (!args.IsUpdateAvailable) return;

                AutoUpdater.ShowUpdateForm(args);
            };
        }
    }
}