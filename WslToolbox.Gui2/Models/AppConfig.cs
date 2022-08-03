using System;

namespace WslToolbox.Gui2.Models;

public class AppConfig
{
    public bool HideDockerDist { get; set; } = true;
    public string DefaultExportFolder { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public AppConfig Clone()
    {
        return (AppConfig) MemberwiseClone();
    }
}