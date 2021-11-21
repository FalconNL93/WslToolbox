using System;
using System.Threading.Tasks;

namespace WslToolbox.Core.Commands.Distribution
{
    public static class OpenShellDistributionCommand
    {
        public static event EventHandler OpenShellInstallDistributionFinished;

        public static async void Execute(DistributionClass distribution)
        {
            CommandClass.StartShell(distribution);

            if (distribution.IsInstalled) return;

            while (!ToolboxClass.DistributionByName(distribution.Name).IsInstalled)
                await Task.Delay(5000);

            OpenShellInstallDistributionFinished?.Invoke(distribution, EventArgs.Empty);
        }
    }
}