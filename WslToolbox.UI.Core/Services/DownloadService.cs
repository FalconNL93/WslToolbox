using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Core.Models;
using System;

namespace WslToolbox.UI.Core.Services;
public class UserProgressChangedEventArgs : EventArgs
{
    public long Progress { get; set; }
    public long TotalBytes { get; set; }
}
public class DownloadService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DownloadService> _logger;
    public static event EventHandler<UserProgressChangedEventArgs> ProgressChanged;

    public DownloadService(HttpClient httpClient, ILogger<DownloadService> logger, IOptions<DevOptions> devOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> DownloadFileAsync(UpdateResultModel updateResultModel)
    {
        var destStream = new MemoryStream();
        var cancellationToken = new CancellationToken();
        var downloadUri = $"{updateResultModel.LatestVersion}/{updateResultModel.Files.Setup}";
        _logger.LogInformation("Trying to download {DownloadUri}", downloadUri);
        using var fileDownload = await _httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead);
        var contentLength = fileDownload.Content.Headers.ContentLength;

        await using var fileStream = await fileDownload.Content.ReadAsStreamAsync(cancellationToken);
        var buffer = new byte[81920];
        long totalBytesRead = 0;
        int bytesRead;
        while ((bytesRead = await fileStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            
            OnProgressChanged(new UserProgressChangedEventArgs
            {
                Progress = totalBytesRead,
                TotalBytes = contentLength ??= 0
            });
        }

        var tempFile = Path.GetTempFileName();
        _logger.LogInformation("Download file to {TempFile}", tempFile);

        return tempFile;
    }

    protected virtual void OnProgressChanged(UserProgressChangedEventArgs e)
    {
        ProgressChanged?.Invoke(this, e);
    }
}