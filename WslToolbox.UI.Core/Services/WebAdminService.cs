using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WslToolbox.Web;

namespace WslToolbox.UI.Core.Services;

public class WebAdminService : BackgroundService
{
    private readonly ILogger<WebAdminService> _logger;
    private readonly WebAdmin _webAdmin = new();

    public WebAdminService(ILogger<WebAdminService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Service started");
                await _webAdmin.Server().RunAsync(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Service stopped");
            await _webAdmin.Server().StopAsync(stoppingToken);
            Environment.Exit(1);
        }
    }
}