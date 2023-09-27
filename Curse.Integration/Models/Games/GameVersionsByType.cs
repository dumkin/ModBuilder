using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Games;

public class GameVersionsByType
{
    [JsonPropertyName("type")]
    public uint Type { get; set; }

    [JsonPropertyName("versions")]
    public List<string> Versions { get; set; } = new();
}