using System;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Games;

public class SortableGameVersion
{
    [JsonPropertyName("gameVersionName")]
    public string GameVersionName { get; set; }

    [JsonPropertyName("gameVersionPadded")]
    public string GameVersionPadded { get; set; }

    [JsonPropertyName("gameVersion")]
    public string GameVersion { get; set; }

    [JsonPropertyName("gameVersionReleaseDate")]
    public DateTimeOffset GameVersionReleaseDate { get; set; }

    [JsonPropertyName("gameVersionTypeId")]
    public uint? GameVersionTypeId { get; set; }
}