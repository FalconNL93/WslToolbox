using System;
using System.Windows.Input;
using ModernWpf.Controls;
using WslToolbox.Core;
using WslToolbox.Gui.Helpers.Ui;

namespace WslToolbox.Gui.Commands.Distribution
{
    public abstract class GenericDistributionCommand : ICommand
    {
        private readonly ContentDialog _waitDialog;
        private readonly WaitHelper _waitHelper;
        protected readonly DistributionClass DistributionClass;
        protected string DefaultInfoContent = "Processing...";
        protected string DefaultInfoTitle = "Please wait";
        protected Func<object, bool> IsExecutable;
        protected Func<object, bool> IsExecutableDefault;

        protected GenericDistributionCommand(DistributionClass distributionClass)
        {
            DistributionClass = distributionClass;

            _waitHelper = new WaitHelper
            {
                ProgressRingActive = true
            };

            _waitDialog = _waitHelper.WaitDialog();
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
            _waitHelper.CloseButtonText = showHideButton ? "Hide" : null;
            _waitHelper.DialogTitle = title ?? DefaultInfoTitle;
            _waitHelper.DialogMessage = content ?? DefaultInfoContent;
            _waitDialog.ShowAsync();
        }

        protected void HideInfo()
        {
            if (_waitDialog.IsVisible)
                _waitDialog.Hide();
        }
    }
}