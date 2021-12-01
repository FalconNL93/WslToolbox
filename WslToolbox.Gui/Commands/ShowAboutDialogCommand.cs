using WslToolbox.Gui.Collections.Dialogs;
using WslToolbox.Gui.Helpers.Ui;
using WslToolbox.Gui.ViewModels;

namespace WslToolbox.Gui.Commands
{
    public class ShowAboutDialogCommand : GenericCommand
    {
        private readonly MainViewModel _viewModel;

        public ShowAboutDialogCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            IsExecutableDefault = _ => true;
            IsExecutable = IsExecutableDefault;
        }

        public override async void Execute(object parameter)
        {
            var selectDistribution = DialogHelper.ShowContentDialog(
                "About",
                AboutDialogCollection.Items(_viewModel),
                null, null, "Close");

            await selectDistribution.Dialog.ShowAsync();
        }
    }
}