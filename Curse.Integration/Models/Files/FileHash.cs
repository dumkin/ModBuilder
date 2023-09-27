using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;

namespace Curse.Integration.Models.Files;

public class FileHash
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("algo")]
    public HashAlgo Algo { get; set; }
}