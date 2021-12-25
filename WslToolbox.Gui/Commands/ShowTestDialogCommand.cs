namespace WslToolbox.Gui.Commands
{
    public class ShowTestDialogCommand : GenericCommand
    {
        public ShowTestDialogCommand()
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override void Execute(object parameter)
        {
        }
    }
}