using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WslToolbox.Core.Legacy;

public class CommandClass
{
    public string Command { get; set; }
    public int ExitCode { get; set; }
    public string Output { get; set; }

    public static CommandClass ExecuteCommand(string command,
        int timeout = 2000,
        bool elevated = false,
        string executable = "cmd.exe",
        bool hidden = true
    )
    {
        CommandClass wslProcess = new()
        {
            Command = command
        };

        Process p = new()
        {
            StartInfo = ProcessStartInfo(command, elevated, executable, hidden)
        };

        p.Start();

        if (elevated)
        {
            p.WaitForExit();
            return wslProcess;
        }

        var reader = p.StandardOutput;
        var output = reader.ReadToEnd();

        wslProcess.ExitCode = p.ExitCode;
        wslProcess.Output = FormatOutput(output);

        return wslProcess;
    }

    private static ProcessStartInfo ProcessStartInfo(string arguments,
        bool elevated = false,
        string executable = "cmd.exe",
        bool hidden = true)
    {
        arguments = hidden
            ? $"/c {arguments}"
            : $"/k {arguments}";

        Debug.WriteLine(arguments);

        return new ProcessStartInfo
        {
            UseShellExecute = elevated,
            WindowStyle = ProcessWindowStyle.Normal,
            FileName = executable,
            Arguments = $"{arguments}",
            CreateNoWindow = !elevated,
            RedirectStandardOutput = !elevated,
            Verb = elevated ? "runas" : string.Empty
        };
    }

    public static void StartShell(DistributionClass distribution)
    {
        if (distribution is null)
        {
            return;
        }

        var shellCommand = $"/c wsl -d {distribution.Name}";

        if (!distribution.IsInstalled)
        {
            shellCommand = $"/c wsl --install -d {distribution.Name}";
        }

        if (distribution.State != DistributionClass.StateRunning && distribution.IsInstalled)
        {
            return;
        }

        Process p = new();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = shellCommand;
        p.Start();
    }

    public static async Task<Process> StartShellAsync(DistributionClass distribution)
    {
        Process p = new();

        if (distribution is null)
        {
            return p;
        }

        var shellCommand = $"/c wsl -d {distribution.Name}";

        if (!distribution.IsInstalled)
        {
            shellCommand = $"/c wsl --install -d {distribution.Name}";
        }

        if (distribution.State != DistributionClass.StateRunning && distribution.IsInstalled)
        {
            return p;
        }

        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.Arguments = shellCommand;
        p.Start();

        await p.WaitForExitAsync();

        return p;
    }

    private static string FormatOutput(string output)
    {
        var formattedOutput = string.Empty;
        output = Regex.Replace(output, "\n", string.Empty);
        output = Regex.Replace(output, "\t", " ");

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