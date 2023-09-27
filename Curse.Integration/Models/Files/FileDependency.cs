using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;

namespace Curse.Integration.Models.Files;

public class FileDependency
{
    [JsonPropertyName("modId")]
    public uint ModId { get; set; }

    [JsonPropertyName("relationType")]
    public FileRelationType RelationType { get; set; }
}