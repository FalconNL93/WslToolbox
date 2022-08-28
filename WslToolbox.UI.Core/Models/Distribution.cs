namespace WslToolbox.UI.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public class Distribution
{
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