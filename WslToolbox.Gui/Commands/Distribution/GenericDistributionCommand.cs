using System;
using System.Windows.Input;
using WslToolbox.Core;

namespace WslToolbox.Gui.Commands.Distribution
{
    public abstract class GenericDistributionCommand : ICommand
    {
        protected readonly DistributionClass DistributionClass;
        protected Func<object, bool> IsExecutable;
        protected Func<object, bool> IsExecutableDefault;

        protected GenericDistributionCommand(DistributionClass distributionClass)
        {
        }

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