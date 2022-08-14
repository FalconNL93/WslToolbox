using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using WslToolbox.Core.Helpers;
using WslToolbox.Gui2.Services;

namespace WslToolbox.Gui2.ViewModels;

public class InformationViewModel : ObservableObject
{
    private readonly ILogger<InformationViewModel> _logger;
    private readonly UpdateService _updateService;
    private string _latestVersion = "0.0.0.0";

    public InformationViewModel(ILogger<InformationViewModel> logger, UpdateService updateService)
    {
        _logger = logger;
        _updateService = updateService;

        CheckForUpdates = new AsyncRelayCommand(OnCheckForUpdates);
        OpenLogFile = new RelayCommand(OnOpenLogFile);
    }

    public string? AppVersion { get; } = App.AssemblyVersionFull;
    public string? CoreVersion { get; } = AssemblyHelper.AssemblyVersionFull;
    public AsyncRelayCommand CheckForUpdates { get; }
    public RelayCommand OpenLogFile { get; }

    public string LatestVersion
    {
        get => _latestVersion;
        private set
        {
            if (value == _latestVersion)
            {
                return;
            }

            _latestVersion = value;
            OnPropertyChanged();
        }
    }

    private async Task OnCheckForUpdates()
    {
        LatestVersion = "Checking for updates...";

        try
        {
            var updateManifest = await _updateService.GetUpdateManifest();
            _logger.LogInformation("Version from manifest: {Version}", updateManifest.Version);
            _updateService.UpdateAvailable(updateManifest);
            LatestVersion = updateManifest.Version;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve update manifest");
        }
    }

    private void OnOpenLogFile()
    {
        try
        {
            var logOpener = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = "\"" + $@"{App.AppDirectory}\logs\{AppDomain.CurrentDomain.FriendlyName}.log" + "\""
                }
            };

            logOpener.Start();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to execute {Path}",
                "\"" + $@"{App.AppDirectory}\logs\{AppDomain.CurrentDomain.FriendlyName}.log" + "\"");
        }
    }
}