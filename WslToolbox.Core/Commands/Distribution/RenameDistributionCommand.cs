using System;
using System.Threading.Tasks;
using WslToolbox.Core.Helpers;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class RenameDistributionCommand
    {
        public static event EventHandler DistributionRenameStarted;
        public static event EventHandler DistributionRenameFinished;

        public static async void Execute(DistributionClass distribution, string newName)
        {
            DistributionRenameStarted?.Invoke(distribution, EventArgs.Empty);
            await TerminateDistributionCommand.Execute(distribution);
            await Task
                .Run(() => { RegistryHelper.ChangeKey(distribution, "DistributionName", newName); })
                .ConfigureAwait(true);

            await StartDistributionCommand.Execute(distribution);
            DistributionRenameFinished?.Invoke(distribution, EventArgs.Empty);
        }
    }
}