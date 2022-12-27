using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class DownloadService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DownloadService> _logger;

    public DownloadService(HttpClient httpClient, ILogger<DownloadService> logger, IOptions<DevOptions> devOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task DownloadFileAsync(UpdateResultModel updateResultModel)
    {
        var updateUrl = new Uri("");
        if (!File.Exists("download.zip"))
        {
        }

        var fileBytes = await _httpClient.GetByteArrayAsync(updateUrl);
        await File.WriteAllBytesAsync("download.zip", fileBytes);
    }
}