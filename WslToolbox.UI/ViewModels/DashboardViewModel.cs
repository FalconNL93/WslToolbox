using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Controls;
using WslToolbox.Core.Legacy.Commands.Distribution;
using WslToolbox.UI.Core.Commands;
using WslToolbox.UI.Core.Helpers;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Extensions;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Views.Modals;

namespace WslToolbox.UI.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly ILogger<DashboardViewModel> _logger;
    private readonly IMessenger _messenger;
    private readonly UserOptions _userOptions;

    [ObservableProperty]
    private bool _isRefreshing = true;

    public DashboardViewModel(
        DistributionService distributionService,
        ILogger<DashboardViewModel> logger,
        IMessenger messenger,
        IOptions<UserOptions> userOptions
    )
    {
        _distributionService = distributionService;
        _logger = logger;
        _messenger = messenger;
        _userOptions = userOptions.Value;

        EventHandlers();
    }

    public static bool CanRenameDistribution => true;
    public static bool CanMoveDistribution => true;

    public OpenUrlCommand OpenUrlCommand { get; } = new();

    public ObservableCollection<Distribution> Distributions { get; set; } = new();

    private static bool DistributionIsRunning(Distribution? distribution)
    {
        return distribution is {State: "Running"};
    }

    private static bool DistributionIsStopped(Distribution? distribution)
    {
        return distribution is {State: "Stopped"};
    }

    private static bool DistributionIsBusy(Distribution? distribution)
    {
        return distribution is {State: "Busy"};
    }

    [RelayCommand]
    private async Task ShowStartupDialog()
    {
        if (_userOptions.SeenWelcomePage)
        {
            return;
        }

        var vm = App.GetService<StartupDialogViewModel>();
        await _messenger.ShowStartupDialogAsync(vm);
    }

    private void EventHandlers()
    {
        WslToolbox.Core.Legacy.Commands.Distribution.StartDistributionCommand.DistributionStartFinished += OnReloadExecution;
        WslToolbox.Core.Legacy.Commands.Distribution.OpenShellDistributionCommand.OpenShellInstallDistributionFinished += OnReloadExecution;
        WslToolbox.Core.Legacy.Commands.Distribution.RenameDistributionCommand.DistributionRenameFinished += OnReloadExecution;
        TerminateDistributionCommand.DistributionTerminateFinished += OnReloadExecution;
        UnregisterDistributionCommand.DistributionUnregisterFinished += OnReloadExecution;

        WslToolbox.Core.Legacy.Commands.Distribution.ImportDistributionCommand.DistributionImportStarted += OnReloadExecution;
        WslToolbox.Core.Legacy.Commands.Distribution.ImportDistributionCommand.DistributionImportFinished += OnReloadExecution;
        WslToolbox.Core.Legacy.Commands.Distribution.ExportDistributionCommand.DistributionExportStarted += OnReloadExecution;
        WslToolbox.Core.Legacy.Commands.Distribution.ExportDistributionCommand.DistributionExportFinished += OnReloadExecution;
    }

    private void OnReloadExecution(object? sender, EventArgs e)
    {
        RefreshDistributionsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task StartAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.StartDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task StopAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.StopDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task RestartAllDistribution()
    {
        foreach (var distribution in Distributions)
        {
            await _distributionService.RestartDistribution(distribution);
        }
    }

    [RelayCommand]
    private async Task AddDistribution(Page page)
    {
        _messenger.ShowInfoBar("Fetching available distributions...");
        var available = await _distributionService.ListInstallableDistributions();
        _messenger.HideInfoBar();
        var distributions = available.Where(x => !x.IsInstalled).ToList();
        if (!distributions.Any())
        {
            _messenger.ShowInfoBar("No distributions available for installation", InfoBarSeverity.Warning);
            return;
        }

        try
        {
            var installDistribution = await page.ShowModal<AddDistribution>(distributions, "Add distribution", "Add");
            if (installDistribution.Modal == null || installDistribution.ContentDialogResult != ContentDialogResult.Primary)
            {
                return;
            }

            _distributionService.InstallDistribution(installDistribution.Modal.GetSelectedItem<Distribution>());
        }
        catch (Exception e)
        {
            _messenger.ShowInfoBar("No distributions available for installation", InfoBarSeverity.Error);
            _logger.LogError(e, "Could not start {Message}", e.Message);
        }
    }

    [RelayCommand]
    private async Task RefreshDistributions()
    {
        _logger.LogInformation("Refreshing list of distributions");
        try
        {
            IsRefreshing = true;
            Distributions.Clear();
            (await _distributionService.ListDistributions()).ToList()
                .ForEach(distribution =>
                {
                    Distributions.Add(distribution);
                });
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand(CanExecute = nameof(DistributionIsRunning))]
    private void OpenShellDistribution(Distribution? distribution)
    {
        _distributionService.OpenShellDistribution(distribution);
    }

    [RelayCommand(CanExecute = nameof(DistributionIsStopped))]
    private async Task DeleteDistribution(Distribution? distribution)
    {
        var deleteConfirm = await _messenger.ShowDialog("Delete", $"Delete {distribution.Name}?", "Delete", "Cancel");
        if (deleteConfirm != ContentDialogResult.Primary)
        {
            return;
        }

        await _distributionService.DeleteDistribution(distribution);
    }

    [RelayCommand]
    private async Task RestartDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
        await _distributionService.StartDistribution(distribution);
    }

    [RelayCommand(CanExecute = nameof(DistributionIsRunning))]
    private async Task StopDistribution(Distribution? distribution)
    {
        await _distributionService.StopDistribution(distribution);
    }

    [RelayCommand(CanExecute = nameof(DistributionIsStopped))]
    private async Task StartDistribution(Distribution? distribution)
    {
        await _distributionService.StartDistribution(distribution);
    }

    [RelayCommand(CanExecute = nameof(CanRenameDistribution))]
    private async Task RenameDistribution(Distribution? distribution)
    {
        var inputRequest = await _messenger.ShowInputDialog("Rename distribution", "Enter a new name for your distribution", distribution.Name, "Rename", "Cancel");
        if (inputRequest.ContentDialogResult != ContentDialogResult.Primary || inputRequest.Result == distribution.Name)
        {
            return;
        }

        distribution.Name = inputRequest.Result;
        await _distributionService.RenameDistributions(new UpdateModel<Distribution>
        {
            CurrentModel = distribution,
            NewModel = distribution
        });
    }

    [RelayCommand(CanExecute = nameof(CanMoveDistribution))]
    private async Task MoveDistribution(Distribution? distribution)
    {
        if (distribution == null)
        {
            _logger.LogError("Invalid distribution");
            return;
        }
        
        var moveSettings = await _messenger.ShowMoveDialog(distribution);
        if (moveSettings.ContentDialogResult != ContentDialogResult.Primary)
        {
            return;
        }

        IoService.CopyDirectoryStatusChanged += (_, args) =>
        {
            _messenger.ShowInfoBar($"Copying file {args.CurrentFile.Name}...", InfoBarSeverity.Informational, $"Moving {distribution.Name}", false);
        };

        IoService.CopyDirectoryFinished += (_, _) =>
        {
            _messenger.ShowInfoBar("Distribution moved", InfoBarSeverity.Success, $"Moving {distribution.Name}");
        };

        try
        {
            await _distributionService.MoveDistribution(distribution, moveSettings.Directory);
        }
        catch (Exception e)
        {
            _messenger.ShowInfoBar("Unable to move distribution", InfoBarSeverity.Error, $"Moving {distribution.Name}");
            await _messenger.ShowDialog("Error", "Unable to move distribution", textBoxMessage: e.Message);
        }
    }

    [RelayCommand]
    private async Task ExportDistribution(Distribution? distribution)
    {
        var filter = DialogHelper.ExtensionFilter(new Dictionary<string, string>
        {
            {FileExtensions.Tar.Name, FileExtensions.Tar.Extension}
        });

        var exportPath = DialogHelper.ShowSaveFileDialog(new SaveFileDialog
        {
            DefaultExt = FileExtensions.Tar.Extension,
            FileName = distribution.Name,
            Filter = filter,
            Title = $"Export {distribution.Name}"
        });

        if (exportPath.Result != DialogResult.OK)
        {
            return;
        }

        await _distributionService.ExportDistribution(distribution, exportPath.Dialog.FileName);
    }

    [RelayCommand]
    private async Task ImportDistribution()
    {
        var filter = DialogHelper.ExtensionFilter(new Dictionary<string, string>
        {
            {FileExtensions.Tar.Name, FileExtensions.Tar.Extension}
        });

        var importPath = DialogHelper.ShowOpenFileDialog(new OpenFileDialog
        {
            DefaultExt = FileExtensions.Tar.Name,
            Filter = filter,
            Title = "Import distribution"
        });

        if (importPath.Result != DialogResult.OK)
        {
            return;
        }

        var importFile = Path.GetFullPath(importPath.Dialog.FileName);
        var fileName = Path.GetFileNameWithoutExtension(importFile);
        var fileInfo = new FileInfo(importFile);
        if (fileInfo.DirectoryName == null)
        {
            return;
        }

        var importSettings = await _messenger.ShowImportDialog(fileName, fileInfo.DirectoryName);
        if (importSettings.ContentDialogResult != ContentDialogResult.Primary)
        {
            return;
        }

        _logger.LogInformation("Trying to import {File} as {Name} ({Bytes})", fileInfo.FullName, importSettings.Name, fileInfo.Length);
        await _distributionService.ImportDistribution(new NewDistributionModel
        {
            Name = importSettings.Name,
            InstallPath = fileInfo.DirectoryName,
            File = importFile
        });
        _logger.LogInformation("Imported {File} as {Name}", fileInfo.FullName, importSettings.Name);
    }
}