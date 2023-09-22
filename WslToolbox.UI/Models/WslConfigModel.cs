namespace WslToolbox.UI.Models;

public class WslConfigModel
{
    public BootSection Boot { get; set; } = new();
    public ExperimentalSection Experimental { get; set; } = new();
}

public class BootSection
{
    public bool? Systemd { get; set; }
}

public class ExperimentalSection
{
    public string? AutoMemoryReclaim { get; set; }
    public bool SparseVhd { get; set; } = false;
}