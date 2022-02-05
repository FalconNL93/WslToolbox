using System.Diagnostics;
using System.IO;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Commands
{
    public class OpenAppFolderCommand : GenericCommand
    {
        public OpenAppFolderCommand()
        {
            IsExecutable = _ => Directory.Exists(AppConfiguration.AppExecutableDirectory);
        }

        public override void Execute(object parameter)
        {
            _ = Process.Start(new ProcessStartInfo("explorer")
            {
                Arguments = Path.GetFullPath(AppConfiguration.AppExecutableDirectory)
            });
        }
    }
}