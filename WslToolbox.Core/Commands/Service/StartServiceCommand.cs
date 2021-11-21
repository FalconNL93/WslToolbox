﻿using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Service
{
    public static class StartServiceCommand
    {
        private const string Command = "wsl --exec exit";

        public static event EventHandler ServiceStartFinished;

        public static async Task Execute()
        {
            var startServiceTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
                Command
            ))).ConfigureAwait(true);

            ServiceStartFinished?.Invoke(null, EventArgs.Empty);
        }
    }
}