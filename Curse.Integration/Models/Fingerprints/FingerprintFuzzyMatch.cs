using System.Collections.Generic;
using System.Text.Json.Serialization;
using Curse.Integration.Models.Files;

namespace Curse.Integration.Models.Fingerprints;

public class FingerprintFuzzyMatch
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("file")]
    public File File { get; set; }

    [JsonPropertyName("latestFiles")]
    public List<File> LatestFiles { get; set; } = new();

    [JsonPropertyName("fingerprints")]
    public List<long> Fingerprints { get; set; } = new();
}