using System.Diagnostics;
using System.Windows.Input;
using WslToolbox.Gui.Commands;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.ViewModels
{
    public class MainViewModel
    {
        private readonly MainView _view;
        public ICommand ShowApplicationCommand => new RelayCommand(o => { _view.WindowState = System.Windows.WindowState.Normal; }, o => true);

        public MainViewModel(MainView view)
        {
            _view = view;
        }
    }
}