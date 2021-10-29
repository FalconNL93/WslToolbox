using System.Windows;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands
{
    public class ShowApplicationCommand : GenericCommand
    {
        private readonly MainView _mainView;

        public ShowApplicationCommand(MainView mainView)
        {
            _mainView = mainView;
            IsExecutable = _ => _mainView.WindowState == WindowState.Minimized;
            IsExecutableDefault = IsExecutable;
        }

        public override void Execute(object parameter)
        {
            _mainView.WindowState = WindowState.Normal;
            _mainView.Show();
        }
    }
}