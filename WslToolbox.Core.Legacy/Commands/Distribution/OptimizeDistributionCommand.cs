using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace WslToolbox.Core.Legacy.Commands.Distribution;

public static class OptimizeDistributionCommand
{
    private const string Command = "diskpart /s {0} > {1}";

    public static Task<CommandClass> Execute(DistributionClass distribution, string file, string output)
    {
        ToolboxClass.OnRefreshRequired();
        var tempFile = WriteDiskPart(file);
        var startTask = CommandClass.ExecuteCommand(string.Format(Command, tempFile, output), elevated: true);
        File.Delete(tempFile);
        ToolboxClass.OnRefreshRequired();

        return Task.FromResult(startTask);
    }

    private static string WriteDiskPart(string basePath)
    {
        var tempFile = Path.GetTempFileName();
        var diskPartScript = new List<string>
        {
            $"select vdisk file=\"{basePath}\"",
            "attach vdisk readonly",
            "detach vdisk"
        };

        File.WriteAllText(tempFile, string.Join(Environment.NewLine, diskPartScript));

        Debug.Write(string.Join("\\n", diskPartScript));
        return tempFile;
    }
}