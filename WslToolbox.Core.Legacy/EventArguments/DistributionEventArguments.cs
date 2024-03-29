﻿using System;

namespace WslToolbox.Core.Legacy.EventArguments;

public class DistributionEventArguments : EventArgs
{
    public readonly string Command;
    public DistributionClass Distribution;

    public DistributionEventArguments(string command, DistributionClass distribution)
    {
        Command = command;
        Distribution = distribution;
    }
}