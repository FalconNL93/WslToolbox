using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Forms;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public partial class Dashboard : INavigableView<DashboardViewModel>
{
    private readonly IDialogControl _dialogControl;
    private readonly ISnackbarService _snackbarService;

    public Dashboard(DashboardViewModel viewModel, IDialogService dialogService, ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
        _dialogControl = dialogService.GetDialogControl();
        ViewModel = viewModel;

        ShowEditDistribution = new AsyncRelayCommand<DistributionModel>(OnEditShowDistribution);

        InitializeComponent();
    }

    public AsyncRelayCommand<DistributionModel> ShowEditDistribution { get; }
    public DashboardViewModel ViewModel { get; }

    private async Task OnEditShowDistribution(DistributionModel? arg)
    {
        if (arg == null)
        {
            return;
        }

        var model = new UpdateModel<DistributionModel>
        {
            CurrentModel = arg,
            NewModel = arg.Clone()
        };

        _dialogControl.Content = new EditDistributionForm {Distribution = model.NewModel};

        _dialogControl.ButtonLeftName = "Save";
        _dialogControl.ButtonRightName = "Cancel";
        var dialogResult = await _dialogControl.ShowAndWaitAsync($"Rename - {model.NewModel.Name}", "Enter a new name for your distribution");

        switch (dialogResult)
        {
            case IDialogControl.ButtonPressed.Left:
                await ViewModel.EditDistribution.ExecuteAsync(model);
                _dialogControl.Hide();
                break;
            case IDialogControl.ButtonPressed.Right:
            case IDialogControl.ButtonPressed.None:
            default:
                _dialogControl.Hide();
                break;
        }
    }
}