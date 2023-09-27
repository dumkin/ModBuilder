using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Games;

public class GameVersion
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}