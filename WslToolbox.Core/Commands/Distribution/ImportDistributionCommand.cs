using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class ImportDistributionCommand
    {
        private const string Command = "wsl --import {0} {1} {2}";

        public static event EventHandler DistributionImportStarted;
        public static event EventHandler DistributionImportFinished;

        public static async void Execute(string name, string path, string file)
        {
            var fireImportEvent = FireImportEvent(name);
            var importTask = ImportAsync(name, path, file);

            await Task.WhenAll(importTask, fireImportEvent);
            DistributionImportFinished?.Invoke(name, EventArgs.Empty);
        }

        private static async Task<CommandClass> ImportAsync(string name, string path, string file)
        {
            var importTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, name, path, file
            ))).ConfigureAwait(true);

            return importTask;
        }

        private static async Task<bool> FireImportEvent(string name)
        {
            await Task.Delay(2000);
            DistributionImportStarted?.Invoke(name, EventArgs.Empty);

            return true;
        }
    }
}