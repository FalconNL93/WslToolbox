using System.Diagnostics;
using System.IO;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Commands
{
    public class OpenLogFileCommand : GenericCommand
    {
        public OpenLogFileCommand()
        {
            IsExecutable = o => File.Exists(LogConfiguration.FileName);
        }

        public override void Execute(object parameter)
        {
            _ = Process.Start(new ProcessStartInfo("explorer")
            {
                Arguments = Path.GetFullPath(LogConfiguration.FileName)
            });
        }
    }
}