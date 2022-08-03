using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Forms;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.ViewModels;

namespace WslToolbox.Gui2.Views.Pages;

public partial class Dashboard : INavigableView<DashboardViewModel>
{
    private readonly IDialogControl _dialogControl;
    private readonly ISnackbarControl _snackbarService;

    public Dashboard(DashboardViewModel viewModel, IDialogService dialogService, ISnackbarService snackbarService)
    {
        _dialogControl = dialogService.GetDialogControl();
        _snackbarService = snackbarService.GetSnackbarControl();
        ViewModel = viewModel;

        ShowEditDistribution = new AsyncRelayCommand<DistributionModel>(OnEditShowDistribution);
        ShowExportDistribution = new AsyncRelayCommand<DistributionModel>(OnExportDistribution);
        ShowDeleteDistribution = new RelayCommand<DistributionModel>(OnDeleteDistribution);

        InitializeComponent();
    }

    public AsyncRelayCommand<DistributionModel> ShowEditDistribution { get; }
    public AsyncRelayCommand<DistributionModel> ShowExportDistribution { get; }
    public RelayCommand<DistributionModel> ShowDeleteDistribution { get; }

    public DashboardViewModel ViewModel { get; }

    private async Task OnExportDistribution(DistributionModel? model) => await ViewModel.ExportDistribution.ExecuteAsync(model);

    private void OnDeleteDistribution(DistributionModel? model)
    {
        var messageBox = new MessageBox
        {
            ButtonLeftName = "Delete",
            ButtonRightName = "Cancel",
            ButtonLeftAppearance = ControlAppearance.Danger,
        };

        messageBox.ButtonLeftClick += async (_, _) => await ViewModel.DeleteDistribution.ExecuteAsync(model);
        messageBox.ButtonRightClick += (_, _) => messageBox.Close();

        messageBox.Show("Delete", $"Are you sure you want to delete {model.Name}?");
    }

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
                ViewModel.EditDistribution.Execute(model);
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