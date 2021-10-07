using System;
using System.Windows.Input;

namespace WslToolbox.Gui.Commands
{
    public abstract class GenericCommand : ICommand
    {
        private protected Func<object, bool> IsExecutable;
        public abstract void Execute(object parameter);

        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return IsExecutable == null || IsExecutable(parameter);
        }
    }
}