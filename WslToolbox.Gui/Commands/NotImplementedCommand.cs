namespace WslToolbox.Gui.Commands
{
    public class NotImplementedCommand : GenericCommand
    {
        public NotImplementedCommand()
        {
            IsExecutable = _ => false;
            IsExecutableDefault = IsExecutable;
        }

        public override void Execute(object parameter)
        {
        }
    }
}