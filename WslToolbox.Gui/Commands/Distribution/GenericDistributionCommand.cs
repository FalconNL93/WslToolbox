using System;
using System.Windows.Input;
using WslToolbox.Core;
using WslToolbox.Gui.Handlers;

namespace WslToolbox.Gui.Commands.Distribution
{
    public abstract class GenericDistributionCommand : ICommand
    {
        private readonly ProgressDialogHandler _progressDialogHandler;
        protected readonly DistributionClass DistributionClass;
        protected string DefaultInfoContent = "Processing...";
        protected string DefaultInfoTitle = "Please wait";
        protected Func<object, bool> IsExecutable;
        protected Func<object, bool> IsExecutableDefault;

        protected GenericDistributionCommand(DistributionClass distributionClass)
        {
            DistributionClass = distributionClass;

            _progressDialogHandler = new ProgressDialogHandler();
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

        protected void ShowInfo(string title = null, string content = null, bool showHideButton = false)
        {
            HideInfo();
            _progressDialogHandler.CloseButtonText = showHideButton ? "Hide" : null;
            _progressDialogHandler.Show(title ?? DefaultInfoTitle, content ?? DefaultInfoContent);
        }

        protected void HideInfo()
        {
            _progressDialogHandler.Dispose();
        }
    }
}