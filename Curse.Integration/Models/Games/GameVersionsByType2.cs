using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Games;

public class GameVersionsByType2
{
    [JsonPropertyName("type")]
    public uint Type { get; set; }

    [JsonPropertyName("versions")]
    public List<GameVersion> Versions { get; set; } = new();
}