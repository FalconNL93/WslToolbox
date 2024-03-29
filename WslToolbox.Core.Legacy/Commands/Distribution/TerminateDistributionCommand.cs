﻿using System;
using System.Threading.Tasks;
using WslToolbox.Core.Legacy.EventArguments;

namespace WslToolbox.Core.Legacy.Commands.Distribution;

public static class TerminateDistributionCommand
{
    private const string Command = "wsl --terminate {0}";

    public static event EventHandler DistributionTerminateStarted;
    public static event EventHandler DistributionTerminateFinished;

    public static async Task<CommandClass> Execute(DistributionClass distribution)
    {
        var args = new DistributionEventArguments(nameof(TerminateDistributionCommand), distribution);

        ToolboxClass.OnRefreshRequired();
        DistributionTerminateStarted?.Invoke(nameof(TerminateDistributionCommand), args);
        var terminateTask = await Task.Run(() => CommandClass.ExecuteCommand(string.Format(
            Command, distribution.Name
        ))).ConfigureAwait(true);
        ToolboxClass.OnRefreshRequired();
        DistributionTerminateFinished?.Invoke(nameof(TerminateDistributionCommand), args);

        return terminateTask;
    }
}