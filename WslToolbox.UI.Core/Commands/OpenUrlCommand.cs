using System.Windows.Input;
using WslToolbox.UI.Core.Helpers;

namespace WslToolbox.UI.Core.Commands;

public class OpenUrlCommand : ICommand
{
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        ShellHelper.OpenFile((string) parameter);
    }

    public event EventHandler CanExecuteChanged;
}