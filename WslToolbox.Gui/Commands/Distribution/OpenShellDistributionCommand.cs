﻿using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class OpenShellDistributionCommand : GenericDistributionCommand
    {
        public OpenShellDistributionCommand(DistributionClass distributionClass) : base(distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override void Execute(object parameter)
        {
            ToolboxClass.ShellDistribution((DistributionClass) parameter);
        }
    }
}