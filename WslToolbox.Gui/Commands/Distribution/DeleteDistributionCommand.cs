using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class DeleteDistributionCommand : GenericDistributionCommand
    {
        public DeleteDistributionCommand(DistributionClass distributionClass) : base(
            distributionClass)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public static event EventHandler DistributionDeleted;

        public override async void Execute(object parameter)
        {
            var distribution = (DistributionClass) parameter;

            await ToolboxClass.UnregisterDistribution(distribution);
            DistributionDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}