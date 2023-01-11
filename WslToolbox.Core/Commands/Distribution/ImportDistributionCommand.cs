using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution;

public static class ImportDistributionCommand
{
    private const string Command = "wsl --import {0} {1} {2}";

    public static event EventHandler DistributionImportStarted;
    public static event EventHandler DistributionImportFinished;

    public static async Task Execute(string name, string installPath, string file)
    {
        await Task.WhenAll(FireImportEvent(name), ImportAsync(name, installPath, file));
        ToolboxClass.OnRefreshRequired();
        DistributionImportFinished?.Invoke(name, EventArgs.Empty);
    }

    private static async Task<CommandClass> ImportAsync(string name, string installPath, string file)
    {
        var importTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
            Command, name, installPath, file
        ))).ConfigureAwait(true);

        return importTask;
    }

    private static async Task<bool> FireImportEvent(string name)
    {
        await Task.Delay(2000);
        ToolboxClass.OnRefreshRequired();
        DistributionImportStarted?.Invoke(name, EventArgs.Empty);

        return true;
    }
}