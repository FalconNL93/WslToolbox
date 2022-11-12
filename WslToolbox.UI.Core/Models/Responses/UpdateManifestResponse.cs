using System.Text.Json.Serialization;

namespace WslToolbox.UI.Core.Models.Responses;

public class UpdateManifestResponse
{
    [JsonPropertyName("version")]
    public string ResponseVersion { get; set; }

    [JsonPropertyName("download")]
    public string DownloadUrl { get; set; }
}