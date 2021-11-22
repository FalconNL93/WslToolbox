using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class StartDistributionCommand
    {
        private const string Command = "wsl --distribution {0} --exec exit";

        public static event EventHandler DistributionStartStarted;
        public static event EventHandler DistributionStartFinished;

        public static async Task<CommandClass> Execute(DistributionClass distribution)
        {
            ToolboxClass.OnRefreshRequired();
            DistributionStartStarted?.Invoke(distribution, EventArgs.Empty);
            var startTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, distribution.Name
            ))).ConfigureAwait(true);
            ToolboxClass.OnRefreshRequired();
            DistributionStartFinished?.Invoke(distribution, EventArgs.Empty);

            return startTask;
        }
    }
}