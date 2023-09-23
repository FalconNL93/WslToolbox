namespace WslToolbox.UI.Models;

public class WslConfigModel
{
    public BootSection Boot { get; set; } = new();
    public ExperimentalSection Experimental { get; set; } = new();
    public NetworkSection Network { get; set; } = new();
    public InteropSection Interop { get; set; } = new();
}

public class BootSection
{
    public bool? Systemd { get; set; }
    public string? Command { get; set; }
}

public class NetworkSection
{
    public bool? GenerateHosts { get; set; }
    public bool? GenerateResolvConf { get; set; }
    public string? Hostname { get; set; }
}

public class ExperimentalSection
{
    public string? AutoMemoryReclaim { get; set; }
    public bool? SparseVhd { get; set; }
    public bool? Firewall { get; set; }
    public bool? DnsTunneling { get; set; }
    public bool? AutoProxy { get; set; }
}

public class InteropSection
{
    public bool? Enabled { get; set; } = true;
    public bool? AppendWindowsPath { get; set; } = true;
}

public class UserSection
{
    public string? Default { get; set; }
}

public class AutomountSection
{
    public bool? Enabled { get; set; }
    public bool? MountFsTab { get; set; }
    public string? Root { get; set; }
    public string? Options { get; set; }
}