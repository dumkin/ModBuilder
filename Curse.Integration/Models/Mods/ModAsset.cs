using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Mods;

public class ModAsset
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("modId")]
    public uint ModId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string ThumbnailUrl { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}