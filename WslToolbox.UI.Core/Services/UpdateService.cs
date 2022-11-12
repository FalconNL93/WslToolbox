using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class UpdateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UpdateService> _logger;
    private const string ManifestFile = "wsltoolbox.json";

    public UpdateService(HttpClient httpClient, ILogger<UpdateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UpdateResultModel> GetUpdateDetails()
    {
        var updateResultModel = new UpdateResultModel();

        try
        {
            var manifest = await _httpClient.GetFromJsonAsync<UpdateManifestResponse>(ManifestFile);
            if (manifest == null)
            {
                return updateResultModel;
            }

            _logger.LogInformation("Could not fetch manifest file");
            updateResultModel = new UpdateResultModel
            {
                LatestVersion = Version.Parse(manifest.ResponseVersion),
                DownloadUri = new Uri(manifest.DownloadUrl),
                LastSearched = DateTime.Now,
                UpdateStatus = updateResultModel.UpdateAvailable ? string.Empty : "No update available"
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not fetch app manifest from {BaseAddress}", _httpClient.BaseAddress);
        }

        return updateResultModel;
    }
}