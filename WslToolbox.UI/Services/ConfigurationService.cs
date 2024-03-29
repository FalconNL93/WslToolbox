﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WslToolbox.UI.Contracts.Services;
using WslToolbox.UI.Core.Helpers;

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
            File.WriteAllText(Toolbox.UserConfiguration, JsonConvert.SerializeObject(configuration));
            _logger.LogInformation("Configuration saved");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not save configuration");
        }
    }

    public void Delete<T>() where T : class
    {
    }

    public void Delete()
    {
        try
        {
            File.Delete(Toolbox.UserConfiguration);
        }
        catch (FileNotFoundException)
        {
            _logger.LogError("Configuration file does not exist");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not delete configuration file");
        }
    }

    public void Restore<T>() where T : class
    {
        Delete();
    }

    public T Read<T>()
    {
        var config = File.ReadAllText(Toolbox.UserConfiguration);

        return JsonConvert.DeserializeObject<T>(config);
    }
}