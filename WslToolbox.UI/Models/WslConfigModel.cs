namespace WslToolbox.UI.Models;

public class WslConfigModel
{
    public BootConfig Boot { get; set; } = new();
    public ExperimentalConfig Experimental { get; set; } = new();
}

public class BootConfig
{
    public bool? Systemd { get; set; }
}

public class ExperimentalConfig
{
    public string? AutoMemoryReclaim { get; set; }
    public bool? SparseVhd { get; set; }
}