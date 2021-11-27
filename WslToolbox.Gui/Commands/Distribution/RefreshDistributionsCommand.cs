using System.Diagnostics;
using System.Threading.Tasks;
using WslToolbox.Gui.Views;

namespace WslToolbox.Gui.Commands.Distribution
{
    public class RefreshDistributionsCommand : GenericCommand
    {
        private MainView _mainView;

        public RefreshDistributionsCommand(MainView mainView)
        {
            _mainView = mainView;
            IsExecutableDefault = o => true;

            IsExecutable = _ => Process.GetProcessesByName("wslhost").Length > 0;
        }

        public override async void Execute(object parameter)
        {
            IsExecutable = _ => false;
            _mainView ??= (MainView) parameter;
            _mainView.PopulateWsl();
            await Task.Delay(2000);
            IsExecutable = IsExecutableDefault;
        }
    }
}