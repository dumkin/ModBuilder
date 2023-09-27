using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Fingerprints;

public class GetFuzzyMatchesRequestBody
{
    [JsonPropertyName("gameId")]
    public uint GameId { get; set; }

    [JsonPropertyName("fingerprints")]
    public List<FolderFingerprint> Fingerprints { get; set; } = new();
}