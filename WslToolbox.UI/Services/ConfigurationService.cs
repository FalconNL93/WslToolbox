using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WslToolbox.UI.Contracts.Services;

namespace WslToolbox.UI.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;

    public ConfigurationService(ILogger<ConfigurationService> logger)
    {
        _logger = logger;
    }

    public void Save<T>(T config) where T : class
    {
        var configuration = new Dictionary<string, object>
        {
            {config.GetType().Name, config}
        };

        try
        {
            File.WriteAllText(App.UserConfiguration, JsonConvert.SerializeObject(configuration));
            _logger.LogInformation("Configuration saved");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not save configuration");
        }
    }

    public T Read<T>()
    {
        var config = File.ReadAllText(App.UserConfiguration);

        return JsonConvert.DeserializeObject<T>(config);
    }
}