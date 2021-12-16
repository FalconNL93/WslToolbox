﻿using System.Threading.Tasks;
using WslToolbox.Gui.Handlers;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands
{
    public class ShowTestDialogCommand : GenericCommand
    {
        private readonly ProgressDialogHandler _progressDialog;

        public ShowTestDialogCommand(MainViewModel viewModel)
        {
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;

            _progressDialog = new ProgressDialogHandler();
        }

        public override async void Execute(object parameter)
        {
            _progressDialog.ShowInfo("Busy", "Please wait...");
            await Task.Delay(5000);
            _progressDialog.UpdateStatus(content: "Almost done!", value: 50);
            await Task.Delay(5000);
            _progressDialog.UpdateStatus(content: "Nearly there!", value: 90);
            await Task.Delay(5000);
            _progressDialog.UpdateStatus(content: "Done!", value: 100);
            await Task.Delay(2000);
            _progressDialog.HideInfo();
        }
    }
}