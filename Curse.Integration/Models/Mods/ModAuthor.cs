using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Mods;

public class ModAuthor
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}