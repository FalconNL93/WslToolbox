namespace WslToolbox.Gui2.Models;

public class DistributionModel
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

    public DistributionModel Clone()
    {
        return (DistributionModel) MemberwiseClone();
    }
}