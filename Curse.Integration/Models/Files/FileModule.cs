using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Files;

public class FileModule
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("fingerprint")]
    public long Fingerpruint { get; set; }
}