using System;
using System.Text.Json.Serialization;

namespace Curse.Integration.Models;

public class Category
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("gameId")]
    public uint GameId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonPropertyName("isClass")]
    public bool? IsClass { get; set; }

    [JsonPropertyName("classId")]
    public uint? ClassId { get; set; }

    [JsonPropertyName("parentCategoryId")]
    public uint? ParentCategoryId { get; set; }

    [JsonPropertyName("displayIndex")]
    public int? DisplayIndex { get; set; }
}