using System;
using System.Threading.Tasks;
using WslToolbox.Core.Legacy.EventArguments;

namespace WslToolbox.Core.Legacy.Commands.Distribution;

public static class ExportDistributionCommand
{
    private const string Command = "wsl --export {0} {1}";

    public static event EventHandler DistributionExportStarted;
    public static event EventHandler DistributionExportFinished;

    public static async Task Execute(DistributionClass distribution, string file)
    {
        var args = new DistributionEventArguments(nameof(ExportDistributionCommand), distribution);

        var fireImportEvent = FireExportEvent(args);
        var exportTask = ExportAsync(distribution, file);

        await Task.WhenAll(exportTask, fireImportEvent);

        ToolboxClass.OnRefreshRequired();
        DistributionExportFinished?.Invoke(typeof(ExportDistributionCommand), args);
    }

    private static async Task<CommandClass> ExportAsync(DistributionClass distribution, string file)
    {
        return await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
            Command, distribution.Name, file
        )));
    }

    private static async Task<bool> FireExportEvent(DistributionEventArguments args)
    {
        await Task.Delay(2000);
        ToolboxClass.OnRefreshRequired();
        DistributionExportStarted?.Invoke(typeof(ExportDistributionCommand), args);

        return true;
    }
}