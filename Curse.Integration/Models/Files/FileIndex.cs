using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;

namespace Curse.Integration.Models.Files;

public class FileIndex
{
    [JsonPropertyName("gameVersion")]
    public string GameVersion { get; set; }

    [JsonPropertyName("fileId")]
    public uint FileId { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("releaseType")]
    public FileReleaseType ReleaseType { get; set; }

    [JsonPropertyName("gameVersionTypeId")]
    public uint? GameVersionTypeId { get; set; }

    [JsonPropertyName("modLoader")]
    public ModLoaderType? ModLoader { get; set; }
}