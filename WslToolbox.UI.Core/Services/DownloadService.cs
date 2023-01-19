using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WslToolbox.UI.Core.Args;
using WslToolbox.UI.Core.Models;
using System;
using WslToolbox.UI.Core.Extensions;

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

    public static event EventHandler<UserProgressChangedEventArgs> ProgressChanged;

    public async Task<string> DownloadFileAsync(UpdateResultModel updateResultModel)
    {
        var cancellationToken = new CancellationToken();
        var tempFile = Path.GetTempFileName();
        _logger.LogDebug("Assigned file for download {TempFile}", tempFile);
        
        await using var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        _logger.LogDebug("Opened file for writing");
        
        var downloadUri = $"{updateResultModel.LatestVersion}/{updateResultModel.Files.Setup}";
        using var fileDownload = await _httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!fileDownload.IsSuccessStatusCode)
        {
            throw new Exception($"Could not fetch {downloadUri}");
        }

        var contentLength = fileDownload.Content.Headers.ContentLength;
        _logger.LogDebug("Got file size of update file {ContentLength}", contentLength?.ToReadableBytes());

        await using var downloadStream = await fileDownload.Content.ReadAsStreamAsync(cancellationToken);
        var buffer = new byte[4096];
        long totalBytesRead = 0;
        int bytesRead;
        
        while ((bytesRead = await downloadStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await fs.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;

            OnProgressChanged(new UserProgressChangedEventArgs
            {
                TotalBytesDownloaded = totalBytesRead,
                TotalBytes = contentLength ??= 0,
                
            });
        }
        
        _logger.LogInformation("Download file to {TempFile}", tempFile);

        return tempFile;
    }

    protected virtual void OnProgressChanged(UserProgressChangedEventArgs e)
    {
        ProgressChanged?.Invoke(this, e);
    }
}