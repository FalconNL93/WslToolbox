using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using WslToolbox.Gui2.Extensions;
using WslToolbox.Gui2.Models;
using WslToolbox.Gui2.Services;
using WslToolbox.Gui2.Views.Forms;

namespace WslToolbox.Gui2.ViewModels;

public class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly IDialogControl _dialogControl;
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly DistributionService _service;
    private readonly ISnackbarService _snackbarService;
    private readonly UpdateService _updateService;

    public DashboardViewModel(
        ILogger<DashboardViewModel> logger,
        DistributionService service,
        ISnackbarService snackbarService,
        IDialogService dialogService,
        UpdateService updateService
    )
    {
        _logger = logger;
        _service = service;
        _snackbarService = snackbarService;
        _updateService = updateService;
        _dialogControl = dialogService.GetDialogControl();

        RefreshDistributions = new AsyncRelayCommand(OnRefreshDistributions);
        StartDistribution = new AsyncRelayCommand<DistributionModel>(OnStartDistribution,
            model => model?.State != DistributionService.StateRunning);
        StopDistribution = new AsyncRelayCommand<DistributionModel>(OnStopDistribution,
            model => model?.State == DistributionService.StateRunning);
        DeleteDistribution = new RelayCommand<DistributionModel>(OnDeleteDistribution);
        ExportDistribution = new AsyncRelayCommand<DistributionModel>(OnExportDistribution);
        EditDistribution = new AsyncRelayCommand<DistributionModel>(OnEditDistribution);
        OptimizeDistribution = new AsyncRelayCommand<DistributionModel>(OnOptimizeDistribution);
    }

    public AsyncRelayCommand RefreshDistributions { get; }
    public AsyncRelayCommand<DistributionModel> StartDistribution { get; }
    public AsyncRelayCommand<DistributionModel> StopDistribution { get; }
    public RelayCommand<DistributionModel> DeleteDistribution { get; }
    public AsyncRelayCommand<DistributionModel> EditDistribution { get; }
    public AsyncRelayCommand<DistributionModel> ExportDistribution { get; }
    public AsyncRelayCommand<DistributionModel> OptimizeDistribution { get; }
    public ObservableCollection<DistributionModel> Distributions { get; } = new();

    public async void OnNavigatedTo()
    {
        await RefreshDistributions.ExecuteAsync(null);
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task OnStartDistribution(DistributionModel distribution)
    {
        await _service.StartDistribution(distribution);
    }

    private async Task OnStopDistribution(DistributionModel distribution)
    {
        await _service.StopDistribution(distribution);
    }

    private async Task OnExportDistribution(DistributionModel distribution)
    {
        await _service.ExportDistribution(distribution);
    }

    private async Task OnOptimizeDistribution(DistributionModel distribution)
    {
        try
        {
            var logFile = await _service.OptimizeDistribution(distribution);
            var log = await File.ReadAllTextAsync(logFile);
            await _dialogControl.ShowAndWaitAsync(new DialogControlModel
            {
                Content = new LogViewerForm
                {
                    Distribution = distribution,
                    Log = log,
                    ReadOnly = true
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to optimize distribution");
        }
    }

    private async Task OnEditDistribution(DistributionModel? distribution)
    {
        if (distribution == null)
        {
            return;
        }

        var model = new UpdateModel<DistributionModel>
        {
            CurrentModel = distribution,
            NewModel = distribution.Clone()
        };

        await _dialogControl.ShowAndWaitAsync(new DialogControlModel
        {
            PrimaryButtonName = "Save",
            Content = new EditDistributionForm {Distribution = distribution},
            PrimaryAction = () => _service.RenameDistributions(model)
        });
    }

    private void OnDeleteDistribution(DistributionModel distribution)
    {
        var messageBox = new MessageBox
        {
            ButtonLeftName = "Delete",
            ButtonRightName = "Cancel",
            ButtonLeftAppearance = ControlAppearance.Danger
        };

        messageBox.ButtonLeftClick += async (_, _) => await _service.DeleteDistribution(distribution);
        messageBox.ButtonRightClick += (_, _) => messageBox.Close();

        messageBox.Show("Delete", $"Are you sure you want to delete {distribution.Name}?");
    }

    private async Task OnRefreshDistributions()
    {
        Distributions.Clear();
        (await _service.ListDistributions()).ToList()
            .ForEach(distribution => { Distributions.Add(distribution); });
    }
}