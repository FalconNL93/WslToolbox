using System.Reflection;
using System.Text.Json.Serialization;

namespace WslToolbox.UI.Core.Models;

public class UpdateResultModel
{
    [JsonPropertyName("version")]
    public string ResponseVersion
    {
        set => LatestVersion = Version.Parse(value);
    }

    public bool IsLatest { get; set; } = true;
    public Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
    public Version LatestVersion { get; private set; }
}