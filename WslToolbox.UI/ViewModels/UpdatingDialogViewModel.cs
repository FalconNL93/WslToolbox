using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WslToolbox.UI.Core.Services;

namespace WslToolbox.UI.ViewModels;

public partial class UpdatingDialogViewModel : ObservableRecipient
{
    private readonly UpdateService _updateService;
    private readonly DownloadService _downloadService;
    private CancellationTokenSource _cancellationTokenSource;

    public UpdatingDialogViewModel(UpdateService updateService, DownloadService downloadService)
    {
        _updateService = updateService;
        _downloadService = downloadService;
        _cancellationTokenSource = new CancellationTokenSource();

        ProgressText = "A new update is available for WSL Toolbox. Press update upgrade to the newest version.";
    }

    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    private string _progressText;

    [RelayCommand]
    private async Task CancelUpdate()
    {
        ProgressText = "Cancelling...";
        await _cancellationTokenSource.CancelAsync();
    }

    [RelayCommand]
    private async Task DownloadUpdate()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var progress = new Progress<double>();
        progress.ProgressChanged += (_, args) =>
        {
            Progress = args;
            ProgressText = $"Downloading update... {args}%";
        };

        ProgressText = "Retrieving update details...";
        var updateManifest = await _updateService.GetUpdateDetails();

        ProgressText = "Downloading update...";
        var downloadedFile = await _downloadService.DownloadFileAsync(updateManifest, progress, _cancellationTokenSource.Token);
    }
}