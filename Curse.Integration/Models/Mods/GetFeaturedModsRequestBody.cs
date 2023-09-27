using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Mods;

public class GetFeaturedModsRequestBody
{
    [JsonPropertyName("gameId")]
    public uint GameId { get; set; }

    [JsonPropertyName("excludedModIds")]
    public List<int> ExcludedModIds { get; set; } = new();

    [JsonPropertyName("gameVersionTypeId")]
    public uint? GameVersionTypeId { get; set; }
}