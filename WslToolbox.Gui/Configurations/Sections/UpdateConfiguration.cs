using AutoUpdaterDotNET;

namespace WslToolbox.Gui.Configurations.Sections
{
    public sealed class UpdateConfiguration
    {
        public readonly string Url = AppConfiguration.AppConfigurationUpdateXml;
        public bool ShowSkipButton { get; } = AutoUpdater.ShowSkipButton;
        public bool Mandatory { get; } = AutoUpdater.Mandatory;
        public bool ShowRemindLaterButton { get; } = AutoUpdater.ShowRemindLaterButton;
        public Mode UpdateMode { get; } = AutoUpdater.UpdateMode;
        public string AppTitle { get; } = AppConfiguration.AppName;
        public bool OpenDownloadPage { get; } = AutoUpdater.OpenDownloadPage;
    }
}