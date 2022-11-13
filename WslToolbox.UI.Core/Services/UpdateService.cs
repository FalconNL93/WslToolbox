using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Models.Responses;

namespace WslToolbox.UI.Core.Services;

public class UpdateService
{
    private const string ManifestFile = "wsltoolbox.json";
    private readonly HttpClient _httpClient;
    private readonly ILogger<UpdateService> _logger;

    public UpdateService(HttpClient httpClient, ILogger<UpdateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UpdateResultModel> GetUpdateDetails()
    {
        var updateResultModel = new UpdateResultModel();
        UpdateManifestResponse manifest;

        try
        {
            manifest = await _httpClient.GetFromJsonAsync<UpdateManifestResponse>(ManifestFile);
            if (manifest == null)
            {
                _logger.LogError("Could not download update manifest");

                updateResultModel.UpdateStatus = "Could not check for updates";
                return updateResultModel;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not fetch app manifest from {BaseAddress}", _httpClient.BaseAddress);

            updateResultModel.UpdateStatus = "Could not check for updates";
            return updateResultModel;
        }

        updateResultModel.LatestVersion = Version.Parse(manifest.ResponseVersion);
        updateResultModel.LastChecked = DateTime.Now;
        updateResultModel.UpdateStatus = updateResultModel.UpdateAvailable ? string.Empty : "No update available";

        if (Uri.IsWellFormedUriString(manifest.DownloadUrl, UriKind.Absolute))
        {
            updateResultModel.DownloadUri = new Uri(manifest.DownloadUrl);
        }

        return updateResultModel;
    }
}