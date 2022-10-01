namespace WslToolbox.UI.Core.Models;

public class Distribution
{
    public const string StateRunning = "Running";
    public const string StateStopped = "Stopped";
    public const string StateAvailable = "Stopped";
    public const string StateBusy = "Busy";

    public bool IsDefault { get; set; }
    public bool IsInstalled { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public int Version { get; set; }
    public string Guid { get; set; }
    public string BasePath { get; set; }
    public string BasePathLocal { get; set; }
    public int DefaultUid { get; set; }
    public long Size { get; set; }

    public Distribution Clone()
    {
        return (Distribution) MemberwiseClone();
    }
}