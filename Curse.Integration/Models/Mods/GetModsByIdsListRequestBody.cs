using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models.Mods;

public class GetModsByIdsListRequestBody
{
    [JsonPropertyName("modIds")]
    public List<uint> ModIds { get; set; } = new();

    [JsonPropertyName("filterPcOnly")]
    public bool? FilterPcOnly { get; set; } = new();
}