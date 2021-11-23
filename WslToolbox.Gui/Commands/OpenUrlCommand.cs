using System;
using System.Diagnostics;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui.Commands
{
    public class OpenUrlCommand : GenericCommand
    {
        public OpenUrlCommand()
        {
            IsExecutable = _ => true;
        }

        public override void Execute(object parameter)
        {
            _ = Process.Start(new ProcessStartInfo("explorer")
            {
                Arguments = (string) parameter
            });
        }
    }
}