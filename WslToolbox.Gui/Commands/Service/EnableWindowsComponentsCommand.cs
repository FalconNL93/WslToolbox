namespace WslToolbox.Gui.Commands.Service
{
    public class EnableWindowsComponentsCommand : GenericCommand
    {
        public EnableWindowsComponentsCommand()
        {
            IsExecutable = _ => true;
        }

        public override async void Execute(object parameter)
        {
            await Core.Commands.Service.EnableWindowsComponentsCommand.Execute();
        }
    }
}