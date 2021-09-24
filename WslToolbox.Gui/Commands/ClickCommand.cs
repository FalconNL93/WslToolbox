using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WslToolbox.Gui.Commands
{
    public class ClickCommandHandler : ICommand
    {
        public ClickCommandHandler()
        {
            Trace.WriteLine("Class Init");
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Trace.WriteLine("Executed!");
        }
    }
}