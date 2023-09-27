using System.Text.Json.Serialization;

namespace Curse.Integration.Models;

public class Pagination
{
    [JsonPropertyName("index")]
    public uint Index { get; set; }

    [JsonPropertyName("pageSize")]
    public uint PageSize { get; set; }

    [JsonPropertyName("resultCount")]
    public uint ResultCount { get; set; }

    [JsonPropertyName("totalCount")]
    public long TotalCount { get; set; }
}