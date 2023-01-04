using System.Text.Json.Serialization;

namespace WslToolbox.UI.Core.Models.Responses;

public class FilesModel
{
    public string Portable { get; set; }
    public string Setup { get; set; }
    
    public string Packaged { get; set; }
}

public class UpdateManifestResponse
{
    [JsonPropertyName("version")]
    public string ResponseVersion { get; set; }

    [JsonPropertyName("download")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("files")]
    public FilesModel Files { get; set; }
}