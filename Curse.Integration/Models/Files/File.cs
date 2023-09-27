﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;
using Curse.Integration.Models.Games;

namespace Curse.Integration.Models.Files;

public class File
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("gameId")]
    public uint GameId { get; set; }

    [JsonPropertyName("modId")]
    public uint ModId { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; set; }

    [JsonPropertyName("releaseType")]
    public FileReleaseType ReleaseType { get; set; }

    [JsonPropertyName("fileStatus")]
    public FileStatus FileStatus { get; set; }

    [JsonPropertyName("hashes")]
    public List<FileHash> Hashes { get; set; } = new();

    [JsonPropertyName("fileDate")]
    public DateTimeOffset FileDate { get; set; }

    [JsonPropertyName("fileLength")]
    public long FileLength { get; set; }

    [JsonPropertyName("downloadCount")]
    public long DownloadCount { get; set; }

    [JsonPropertyName("fileSizeOnDisk")]
    public long? FileSizeOnDisk { get; set; }

    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("gameVersions")]
    public List<string> GameVersions { get; set; } = new();

    [JsonPropertyName("sortableGameVersions")]
    public List<SortableGameVersion> SortableGameVersions { get; set; } = new();

    [JsonPropertyName("dependencies")]
    public List<FileDependency> Dependencies { get; set; } = new();

    [JsonPropertyName("exposeAsAlternative")]
    public bool? ExposeAsAlternative { get; set; }

    [JsonPropertyName("parentProjectFileId")]
    public uint? ParentProjectFileId { get; set; }

    [JsonPropertyName("alternateFileId")]
    public uint? AlternateFileId { get; set; }

    [JsonPropertyName("isServerPack")]
    public bool? IsServerPack { get; set; }

    [JsonPropertyName("serverPackFileId")]
    public uint? ServerPackFileId { get; set; }

    [JsonPropertyName("isEarlyAccessContent")]
    public bool? IsEarlyAccessContent { get; set; }

    [JsonPropertyName("earlyAccessEndDate")]
    public DateTimeOffset? EarlyAccessEndDate { get; set; }

    [JsonPropertyName("fileFingerprint")]
    public long FileFingerpruint { get; set; }

    [JsonPropertyName("modules")]
    public List<FileModule> Modules { get; set; } = new();
}