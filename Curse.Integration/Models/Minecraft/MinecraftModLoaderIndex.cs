using System;
using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;

namespace Curse.Integration.Models.Minecraft;

public class MinecraftModLoaderIndex
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("gameVersion")]
    public string GameVersion { get; set; }

    [JsonPropertyName("latest")]
    public bool Latest { get; set; }

    [JsonPropertyName("recommended")]
    public bool Recommended { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonPropertyName("type")]
    public ModLoaderType Type { get; set; }
}