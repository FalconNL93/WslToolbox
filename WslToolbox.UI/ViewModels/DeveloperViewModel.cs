using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;
using WslToolbox.UI.Services;

namespace WslToolbox.UI.ViewModels;

public partial class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly DownloadService _downloadService;
    private readonly ILogger<DeveloperViewModel> _logger;
    private readonly IMessenger _messenger;
    private readonly UpdateService _updateService;
    private readonly IConfigurationService _configurationService;
    public readonly IOptions<DevOptions> DevOptions;
    public readonly bool IsDebug;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService,
        IMessenger messenger,
        IOptions<DevOptions> devOptions,
        DownloadService downloadService,
        UpdateService updateService,
        IConfigurationService configurationService
    )
    {
        _logger = logger;
        _distributionService = distributionService;
        _messenger = messenger;
        DevOptions = devOptions;
        _downloadService = downloadService;
        _updateService = updateService;
        _configurationService = configurationService;

#if DEBUG
        IsDebug = true;
#endif
    }

    [ObservableProperty]
    private double _downloadProgress;

    [ObservableProperty]
    private bool _updateAvailableOnBoot;

    [RelayCommand]
    private void SaveConfiguration()
    {
        DevOptions.Value.UpdateAvailableOnBoot = UpdateAvailableOnBoot;
        _configurationService.Save(DevOptions.Value);
    }

    public ObservableCollection<string> FakeUpdateResults { get; set; } = new(Enum.GetNames(typeof(FakeUpdateResult)));

    [RelayCommand]
    private async Task ShowStartupDialog()
    {
        var vm = App.GetService<StartupDialogViewModel>();
        await _messenger.ShowStartupDialogAsync(vm);
    }

    [RelayCommand]
    private async Task ShowUpdatingDialog()
    {
        var vm = App.GetService<UpdatingDialogViewModel>();
        await _messenger.ShowUpdatingDialogAsync(vm);
    }

    [RelayCommand]
    private async Task DownloadUpdate()
    {
        var progress = new Progress<double>();
        progress.ProgressChanged += (_, args) =>
        {
            DownloadProgress = args;
        };
        var updateManifest = await _updateService.GetUpdateDetails();
        var cancellationToken = new CancellationTokenSource();
        var downloadedFile = await _downloadService.DownloadFileAsync(updateManifest, progress, cancellationToken.Token);
    }
}