using System.Collections.Generic;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Configurations
{
    public class ExperimentalConfiguration
    {
        public const int ShellBackendCommandPrompt = 0;
        public const int ShellBackendPowerShellCore = 1;
        public const int ShellBackendWindowsPowerShell = 2;
        private int _shellBackends = ShellBackendCommandPrompt;
        public bool ShowExperimentalSettings { get; set; }

        public int ShellBackend
        {
            get => _shellBackends;
            set => _shellBackends = ShellBackendValues().ContainsKey(value) ? value : ShellBackendCommandPrompt;
        }

        private static Dictionary<int, string> DefaultShellBackendValues()
        {
            return new Dictionary<int, string>
            {
                {ShellBackendCommandPrompt, "Command Prompt"}
                // {ShellBackendPowerShellCore, "PowerShell"},
                // {ShellBackendWindowsPowerShell, "Windows PowerShell"}
            };
        }

        public static Dictionary<int, string> ShellBackendValues()
        {
            var values = DefaultShellBackendValues();

            if (EnvHelper.ExecutableEnvironment("cmd.exe") == null)
                values.Remove(ShellBackendCommandPrompt);

            if (EnvHelper.ExecutableEnvironment("pwsh.exe") == null)
                values.Remove(ShellBackendPowerShellCore);

            if (EnvHelper.ExecutableEnvironment("powershell.exe") == null)
                values.Remove(ShellBackendWindowsPowerShell);

            return values;
        }
    }
}