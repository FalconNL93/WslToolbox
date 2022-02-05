namespace WslToolbox.Gui.Configurations.Sections
{
    public sealed class UpdateConfiguration
    {
        public readonly string Url = AppConfiguration.AppConfigurationUpdateXml;
        public bool ShowSkipButton { get; } = true;
        public bool Mandatory { get; } = false;
        public bool ShowRemindLaterButton { get; } = false;
        public object UpdateMode { get; } = null;
        public string AppTitle { get; } = AppConfiguration.AppName;
        public bool OpenDownloadPage { get; } = false;
    }
}