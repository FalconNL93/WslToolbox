using System;
using System.Threading.Tasks;
using WslToolbox.Core.Helpers;

namespace WslToolbox.Core.Commands.Distribution;

public static class RenameDistributionCommand
{
    public static event EventHandler DistributionRenameStarted;
    public static event EventHandler DistributionRenameFinished;

    public static async Task Execute(DistributionClass distribution, string newName)
    {
        ToolboxClass.OnRefreshRequired();
        DistributionRenameStarted?.Invoke(distribution, EventArgs.Empty);
        await TerminateDistributionCommand.Execute(distribution);
        await Task
            .Run(() =>
            {
                RegistryHelper.ChangeKey(distribution, "DistributionName", newName);
            })
            .ConfigureAwait(true);

        await StartDistributionCommand.Execute(distribution);
        ToolboxClass.OnRefreshRequired();
        DistributionRenameFinished?.Invoke(distribution, EventArgs.Empty);
    }
}