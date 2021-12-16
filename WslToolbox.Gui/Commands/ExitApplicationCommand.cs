using System;

namespace WslToolbox.Gui.Commands
{
    public class ExitApplicationCommand : GenericCommand
    {
        public ExitApplicationCommand()
        {
            IsExecutable = _ => true;
        }

        public override void Execute(object parameter)
        {
            parameter ??= 1;
            Environment.Exit((int) parameter);
        }
    }
}