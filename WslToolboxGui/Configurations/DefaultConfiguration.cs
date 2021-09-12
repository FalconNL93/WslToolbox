namespace WslToolboxGui.Configurations
{
    public class DefaultConfiguration
    {
        public bool DebugLogging { get; set; }
        public bool EnableSystemTray { get; set; }
        public bool HideDockerDistributions { get; set; } = true;
        public bool OutputOnStartup { get; set; }
    }
}