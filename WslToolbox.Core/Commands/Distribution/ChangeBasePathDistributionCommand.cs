using System;
using System.Threading.Tasks;
using WslToolbox.Core.Helpers;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class ChangeBasePathDistributionCommand
    {
        public static event EventHandler DistributionChangeBasePathStarted;
        public static event EventHandler DistributionChangeBasePathFinished;

        public static async void Execute(DistributionClass distribution, string newBasePath)
        {
            ToolboxClass.OnRefreshRequired();
            DistributionChangeBasePathStarted?.Invoke(distribution, EventArgs.Empty);
            await TerminateDistributionCommand.Execute(distribution);
            await Task
                .Run(() =>
                {
                    RegistryHelper.ChangeKey(distribution, "BasePath", newBasePath);
                })
                .ConfigureAwait(true);

            await StartDistributionCommand.Execute(distribution);
            ToolboxClass.OnRefreshRequired();
            DistributionChangeBasePathFinished?.Invoke(distribution, EventArgs.Empty);
        }
    }
}