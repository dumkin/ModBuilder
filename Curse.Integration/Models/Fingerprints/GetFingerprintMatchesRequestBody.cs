using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Fingerprints;

public class GetFingerprintMatchesRequestBody
{
    [JsonPropertyName("fingerprints")]
    public List<int> Fingerprints { get; set; } = new();
}