using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Fingerprints;

public class FingerprintFuzzyMatchResult
{
    [JsonPropertyName("fuzzyMatches")]
    public List<FingerprintFuzzyMatch> FuzzyMatches { get; set; } = new();
}