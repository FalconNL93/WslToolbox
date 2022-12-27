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

    public async Task<string> DownloadFileAsync(UpdateResultModel updateResultModel)
    {
        var downloadUri = $"{updateResultModel.LatestVersion}/{updateResultModel.Files.Setup}ss";
        _logger.LogInformation("Trying to download {DownloadUri}", downloadUri);
        var fileBytes = await _httpClient.GetByteArrayAsync(downloadUri);
        var tempFile = Path.GetTempFileName();
        _logger.LogInformation("Download file to {TempFile}", tempFile);
        await File.WriteAllBytesAsync(tempFile, fileBytes);

        return tempFile;
    }
}