using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WslToolbox
{
    internal class CommandClass
    {
        public string Command { get; set; }
        public int ExitCode { get; set; }
        public string Output { get; set; }

        public static CommandClass ExecuteCommand(string command)
        {
            Process p = new();
            CommandClass wslProces = new();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "pwsh.exe";
            p.StartInfo.Arguments = $"-Command {command}";

            p.Start();
            var reader = p.StandardOutput;
            var output = reader.ReadToEnd();

            p.WaitForExit();

            wslProces.Command = command;
            wslProces.ExitCode = p.ExitCode;
            wslProces.Output = FormatOutput(output);

            return wslProces;
        }

        public static void StartShell(DistributionClass distribution)
        {
            string shellCommand = $"-Command wsl -d {distribution.Name}";

            if(!distribution.isInstalled)
            {
                shellCommand = $"-Command wsl --install -d {distribution.Name}";
            }

            if(distribution.State != "Running" && distribution.isInstalled)
            {
                return;
            }

            Process p = new();
            p.StartInfo.FileName = "pwsh.exe";
            p.StartInfo.Arguments = shellCommand;
            p.Start();
        }

        private static string FormatOutput(string output)
        {
            var formattedOutput = string.Empty;
            output = Regex.Replace(output, "\n", string.Empty);

            using (var reader = new StringReader(output))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    line = Regex.Replace(line, @"\p{C}+", string.Empty);
                    line = Regex.Replace(line, '\x20' + "{1,}", "\t");
                    formattedOutput = formattedOutput + line + Environment.NewLine;
                }
            }

            return formattedOutput.Trim();
        }
    }
}