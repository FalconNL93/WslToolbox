using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Services;

public class UpdateService
{
    private const string ManifestBaseUrl = "https://gist.githubusercontent.com/FalconNL93/d88fc34b7ae6928e7a7b3bce5ad5b5a0/raw/";
    private readonly HttpClient _httpClient;
    private readonly ILogger<UpdateService> _logger;

    public UpdateService(ILogger<UpdateService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ManifestBaseUrl),
            Timeout = TimeSpan.FromSeconds(60)
        };
    }

    public async Task<UpdateManifestModel> GetUpdateManifest()
    {
        try
        {
            var updateFile = await _httpClient.GetAsync("update.json");
            var response = await updateFile.Content.ReadAsStringAsync();
            var updaterModel = JsonSerializer.Deserialize<UpdateManifestModel>(response);

            return updaterModel ?? new UpdateManifestModel();
        }
        catch (Exception e)
        {
            throw new Exception("Could not fetch update information", e);
        }
    }

    public void UpdateAvailable(UpdateManifestModel manifestModel)
    {
        var currentVersion = new Version(App.AssemblyVersionFull);
        var latestVersion = new Version(manifestModel.Version);

        var result = currentVersion.CompareTo(latestVersion);
        switch (result)
        {
            case > 0:
                _logger.LogInformation("Application is up to date (2)");
                break;
            case < 0:
                _logger.LogInformation("Newer version available");
                break;
            default:
                _logger.LogInformation("Application is up to date (1)");
                break;
        }
    }
}