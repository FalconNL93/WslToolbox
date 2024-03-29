﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Services;
using WslToolbox.UI.Helpers;

namespace WslToolbox.UI.ViewModels;

public partial class DeveloperViewModel : ObservableRecipient
{
    private readonly DistributionService _distributionService;
    private readonly DownloadService _downloadService;
    private readonly ILogger<DeveloperViewModel> _logger;
    private readonly IMessenger _messenger;
    private readonly UpdateService _updateService;
    public readonly IOptions<DevOptions> DevOptions;
    public readonly bool IsDebug;

    public DeveloperViewModel(
        ILogger<DeveloperViewModel> logger,
        DistributionService distributionService,
        IMessenger messenger,
        IOptions<DevOptions> devOptions,
        DownloadService downloadService,
        UpdateService updateService
    )
    {
        _logger = logger;
        _distributionService = distributionService;
        _messenger = messenger;
        DevOptions = devOptions;
        _downloadService = downloadService;
        _updateService = updateService;

        DownloadService.ProgressChanged += (_, args) =>
        {
            _messenger.ProgressChanged(args);
        };

#if DEBUG
        IsDebug = true;
#endif
    }

    public ObservableCollection<string> FakeUpdateResults { get; set; } = new(Enum.GetNames(typeof(FakeUpdateResult)));

    [RelayCommand]
    private async Task ShowStartupDialog()
    {
        var vm = App.GetService<StartupDialogViewModel>();
        await _messenger.ShowStartupDialogAsync(vm);
    }

    [RelayCommand]
    private async Task DownloadUpdate()
    {
        _messenger.ShowInfoBar();
        var updateManifest = await _updateService.GetUpdateDetails();
        var downloadedFile = await _downloadService.DownloadFileAsync(updateManifest);
    }
}