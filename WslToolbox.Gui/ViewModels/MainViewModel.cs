using System.Windows.Input;
using WslToolbox.Gui.Commands;

namespace WslToolbox.Gui.ViewModels
{
    public class MainViewModel
    {
        private ICommand _clickCommand;
        public ICommand ClickCommand => _clickCommand ??= new ClickCommandHandler();
    }
}