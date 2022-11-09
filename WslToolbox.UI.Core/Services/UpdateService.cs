using System.Net.Http.Json;
using Newtonsoft.Json;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Services;

public class UpdateService
{
    private readonly HttpClient _httpClient;

    public UpdateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UpdateResultModel> LatestVersion()
    {
        var updateResultModel = new UpdateResultModel();
        
        try
        {
            updateResultModel = await _httpClient.GetFromJsonAsync<UpdateResultModel>("wsltoolbox.json");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return updateResultModel;
    }
}