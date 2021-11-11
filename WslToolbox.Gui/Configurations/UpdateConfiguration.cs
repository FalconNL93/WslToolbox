using AutoUpdaterDotNET;

namespace WslToolbox.Gui.Configurations
{
    public class UpdateConfiguration
    {
        public readonly string Url = AppConfiguration.AppConfigurationUpdateXml;
        public bool ShowSkipButton { get; set; } = AutoUpdater.ShowSkipButton;
        public bool Mandatory { get; set; } = AutoUpdater.Mandatory;
        public bool ShowRemindLaterButton { get; set; } = AutoUpdater.ShowRemindLaterButton;
        public Mode UpdateMode { get; set; } = AutoUpdater.UpdateMode;
        public string AppTitle { get; set; } = AppConfiguration.AppName;
        public bool OpenDownloadPage { get; set; } = AutoUpdater.OpenDownloadPage;
    }
}