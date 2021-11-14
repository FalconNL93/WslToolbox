﻿using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class UnregisterDistributionCommand
    {
        private const string Command = "wsl --unregister {0}";

        public static event EventHandler DistributionUnregisterStarted;
        public static event EventHandler DistributionUnregisterFinished;

        public static async Task<CommandClass> Execute(DistributionClass distribution)
        {
            DistributionUnregisterStarted?.Invoke(distribution, EventArgs.Empty);
            var unregisterTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command, distribution.Name
            ))).ConfigureAwait(true);
            DistributionUnregisterFinished?.Invoke(distribution, EventArgs.Empty);

            return unregisterTask;
        }
    }
}