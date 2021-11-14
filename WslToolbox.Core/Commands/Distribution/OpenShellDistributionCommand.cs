namespace WslToolbox.Core.Commands.Distribution
{
    public static class OpenShellDistributionCommand
    {
        public static void Execute(DistributionClass distribution)
        {
            CommandClass.StartShell(distribution);
        }
    }
}