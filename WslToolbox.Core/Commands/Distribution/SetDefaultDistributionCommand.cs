using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class SetDefaultDistributionCommand
    {
        private const string Command = "wsl --set-default {0}";

        public static event EventHandler DistributionDefaultSet;

        public static async Task<CommandClass> Execute(DistributionClass distribution)
        {
            var defaultTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, distribution.Name
            ))).ConfigureAwait(true);

            ToolboxClass.OnRefreshRequired();
            DistributionDefaultSet?.Invoke(distribution, EventArgs.Empty);

            return defaultTask;
        }
    }
}