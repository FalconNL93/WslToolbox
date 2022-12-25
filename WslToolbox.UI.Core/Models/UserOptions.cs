namespace WslToolbox.UI.Core.Models;

public class UserOptions
{
    public bool SeenWelcomePage { get; set; }
    public bool HideDocker { get; set; } = true;
    public bool Analytics { get; set; }
    public string Theme { get; set; } = "Default";
}