using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Core.Models;
using WslToolbox.UI.Core.Models.Responses;

namespace WslToolbox.UI.Core.Services;

public class UpdateService
{
    private const string ManifestFile = "wsltoolbox/manifest.json";
    private readonly HttpClient _httpClient;
    private readonly ILogger<UpdateService> _logger;
    private readonly IOptions<DevOptions> _devOptions;

    public UpdateService(HttpClient httpClient, ILogger<UpdateService> logger, IOptions<DevOptions> devOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
        _devOptions = devOptions;
    }

    public static async Task<bool> UpdateServiceStatus()
    {
        try
        {
            var httpClient = new HttpClient();
            await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri("https://www.github.com"),
            });

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<UpdateResultModel> GetUpdateDetails()
    {
        var updateResultModel = new UpdateResultModel();
        UpdateManifestResponse manifest;
        var enableFaker = _devOptions.Value.FakeUpdateResult != FakeUpdateResult.Off;

        try
        {
            manifest = await _httpClient.GetFromJsonAsync<UpdateManifestResponse>(ManifestFile);
            if (manifest == null)
            {
                _logger.LogError("Could not download update manifest");

                updateResultModel.UpdateStatus = "Could not check for updates";
                updateResultModel.HasError = true;

                return updateResultModel;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not fetch app manifest from {BaseAddress}", _httpClient.BaseAddress);

            updateResultModel.UpdateStatus = "Could not check for updates";
            updateResultModel.IsChecking = false;
            updateResultModel.HasError = true;

            return updateResultModel;
        }
        finally
        {
            updateResultModel.IsChecking = false;
        }

        updateResultModel.LatestVersion = Version.Parse(manifest.ResponseVersion);
        updateResultModel.LastChecked = DateTime.Now;
        updateResultModel.Files = manifest.Files;

        if (enableFaker)
        {
            updateResultModel.LatestVersion = _devOptions.Value.FakeUpdateResult == FakeUpdateResult.NoUpdate ? new Version("0.0.0") : new Version("9.99.99");
        }

        updateResultModel.UpdateStatus = updateResultModel.UpdateAvailable ? string.Empty : "No update available";

        if (Uri.IsWellFormedUriString(manifest.DownloadUrl, UriKind.Absolute))
        {
            updateResultModel.DownloadUri = new Uri(manifest.DownloadUrl);
        }

        return updateResultModel;
    }
}