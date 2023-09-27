using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Fingerprints;

public class FingerprintsMatchesResult
{
    [JsonPropertyName("isCacheBuilt")]
    public bool IsCacheBuilt { get; set; }

    [JsonPropertyName("exactMatches")]
    public List<FingerprintMatch> ExactMatches { get; set; } = new();

    [JsonPropertyName("exactFingerprints")]
    public List<long> ExactFingerprints { get; set; } = new();

    [JsonPropertyName("partialMatches")]
    public List<FingerprintMatch> PartialMatches { get; set; } = new();

    [JsonPropertyName("partialMatchFingerprints")]
    public object PartialMatchFingerprints { get; set; }

    [JsonPropertyName("additionalProperties")]
    public List<long> AdditionalProperties { get; set; } = new();

    [JsonPropertyName("installedFingerprints")]
    public List<long> InstalledFingerprints { get; set; } = new();

    [JsonPropertyName("unmatchedFingerprints")]
    public List<long> UnmatchedFingerprints { get; set; } = new();
}