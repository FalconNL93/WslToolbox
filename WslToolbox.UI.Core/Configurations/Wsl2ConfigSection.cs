using System.Collections.ObjectModel;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Configurations;

public class Wsl2ConfigSection : WslSettingSectionBase
{
    public override string SectionName { get; } = "wsl2";

    public override List<WslSetting> Settings { get; } = new()
    {
        new WslSetting
        {
            Section = "wsl2",
            Key = "kernel",
            Default = null,
            Description = "An absolute Windows path to a custom Linux kernel."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "memory",
            Default = null,
            Description = "How much memory to assign to the WSL 2 VM."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "processors",
            Default = null,
            Description = "How many logical processors to assign to the WSL 2 VM."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "localhostForwarding",
            Default = true,
            Description = "Boolean specifying if ports bound to wildcard or localhost in the WSL 2 VM should be connectable from the host via localhost:port."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "kernelCommandLine",
            Default = @"Blank",
            Description = "Additional kernel command line arguments."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "safeMode",
            Default = false,
            Description = "Run WSL in Safe Mode which disables many features and is intended to be used to recover distributions that are in bad states. Only available for Windows 11 and WSL version 0.66.2+."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "swap",
            Default = null,
            Description = "How much swap space to add to the WSL 2 VM, 0 for no swap file. Swap storage is disk-based RAM used when memory demand exceeds limit on hardware device."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "swapFile",
            Default = @"%USERPROFILE%\AppData\Local\Temp\swap.vhdx",
            Description = "An absolute Windows path to the swap virtual hard disk."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "pageReporting",
            Default = true,
            Description = "Default true setting enables Windows to reclaim unused memory allocated to WSL 2 virtual machine."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "guiApplications",
            Default = true,
            Description = "Boolean to turn on or off support for GUI applications (WSLg) in WSL."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "debugConsole",
            Default = false,
            Description = "Boolean to turn on an output console Window that shows the contents of dmesg upon start of a WSL 2 distro instance. Only available for Windows 11."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "nestedVirtualization",
            Default = true,
            Description = "Boolean to turn on or off nested virtualization, enabling other nested VMs to run inside WSL 2. Only available for Windows 11."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "vmIdleTimeout",
            Default = @"60000",
            Description = "The number of milliseconds that a VM is idle, before it is shut down. Only available for Windows 11."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "dnsProxy",
            Default = true,
            Description = "Only applicable to networkingMode = NAT. Boolean to inform WSL to configure the DNS Server in Linux to the NAT on the host. Setting to false will mirror DNS servers from Windows to Linux."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "networkingMode",
            Default = "NAT",
            Description = "If the value is mirrored then this turns on mirrored networking mode. Default or unrecognized strings result in NAT networking.",
            Options = new ObservableCollection<string>
            {
                "NAT",
                "mirrored"
            }
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "firewall",
            Default = true,
            Description = "Setting this to true allows the Windows Firewall rules, as well as rules specific to Hyper-V traffic, to filter WSL network traffic."
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "dnsTunneling",
            Default = true,
            Description = "Changes how DNS requests are proxied from WSL to Windows"
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "autoProxy",
            Default = true,
            Description = "Enforces WSL to use Windows’ HTTP proxy information"
        },
        new WslSetting
        {
            Section = "wsl2",
            Key = "defaultVhdSize",
            Default = null,
            Description = "Set the Virtual Hard Disk (VHD) size that stores the Linux distribution (for example, Ubuntu) file system. Can be used to limit the maximum size that a distribution file system is allowed to take up."
        }
    };
}