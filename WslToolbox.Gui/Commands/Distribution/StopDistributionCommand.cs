﻿using System;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class StopDistributionCommand : GenericDistributionCommand
    {
        public StopDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionStopped;

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _ = await ToolboxClass.TerminateDistribution((DistributionClass) parameter);
            IsExecutable = _ => true;

            DistributionStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}