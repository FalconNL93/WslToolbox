namespace WslToolbox.Gui.Configurations
{
    public class NotificationConfiguration
    {
        public bool Enabled { get; set; } = true;
        public bool NewVersionAvailable { get; set; } = true;
        public bool ExportFinished { get; set; } = true;
        public bool ImportFinished { get; set; } = true;
    }
}