namespace WslToolbox.Gui2.Models;

public class AppConfig
{
    public bool HideDockerDist { get; set; }

    public AppConfig Clone()
    {
        return (AppConfig) MemberwiseClone();
    }
}