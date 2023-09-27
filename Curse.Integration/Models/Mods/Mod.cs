using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;
using Curse.Integration.Models.Files;

namespace Curse.Integration.Models.Mods;

public class Mod
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("gameId")]
    public uint GameId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("links")]
    public ModLinks Links { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("status")]
    public ModStatus Status { get; set; }

    [JsonPropertyName("downloadCount")]
    public long DownloadCount { get; set; }

    [JsonPropertyName("isFeatured")]
    public bool IsFeatured { get; set; }

    [JsonPropertyName("primaryCategoryId")]
    public uint PrimaryCategoryId { get; set; }

    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; } = new();

    [JsonPropertyName("classId")]
    public uint? ClassId { get; set; }

    [JsonPropertyName("authors")]
    public List<ModAuthor> Authors { get; set; } = new();

    [JsonPropertyName("logo")]
    public ModAsset Logo { get; set; }

    [JsonPropertyName("screenshots")]
    public List<ModAsset> Screenshots { get; set; } = new();

    [JsonPropertyName("mainFileId")]
    public uint MainFileId { get; set; }

    [JsonPropertyName("latestFiles")]
    public List<File> LatestFiles { get; set; } = new();

    [JsonPropertyName("latestFilesIndexes")]
    public List<FileIndex> LatestFilesIndexes { get; set; } = new();

    [JsonPropertyName("latestEarlyAccessFilesIndexes")]
    public List<FileIndex> LatestEarlyAccessFilesIndexes { get; set; } = new();

    [JsonPropertyName("dateCreated")]
    public DateTimeOffset DateCreated { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonPropertyName("dateReleased")]
    public DateTimeOffset DateReleased { get; set; }

    [JsonPropertyName("allowModDistribution")]
    public bool? AllowModDistribution { get; set; }

    [JsonPropertyName("gamePopularityRank")]
    public uint GamePopularityRank { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; }

    [JsonPropertyName("thumbsUpCount")]
    public uint ThumbsUpCount { get; set; }
}