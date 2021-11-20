using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class ImportDistributionCommand
    {
        private const string Command = "wsl --import {0} {1} {2}";

        public static event EventHandler DistributionImportStarted;
        public static event EventHandler DistributionImportFinished;

        public static async Task<CommandClass> Execute(string name, string path, string file)
        {
            DistributionImportStarted?.Invoke(name, EventArgs.Empty);
            var importTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, name, path, file
            ))).ConfigureAwait(true);
            DistributionImportFinished?.Invoke(name, EventArgs.Empty);

            return importTask;
        }
    }
}