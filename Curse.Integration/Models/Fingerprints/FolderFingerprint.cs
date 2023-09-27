using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Fingerprints;

public class FolderFingerprint
{
    [JsonPropertyName("foldername")]
    public string FolderName { get; set; }

    [JsonPropertyName("fingerprints")]
    public List<long> Fingerprints { get; set; } = new();
}