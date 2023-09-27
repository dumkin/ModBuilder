using System;
using System.Text.Json.Serialization;
using Curse.Integration.Models.Enums;

namespace Curse.Integration.Models.Minecraft;

public class MinecraftModloaderInfo
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }

    [JsonPropertyName("gameVersionId")]
    public uint GameVersionId { get; set; }

    [JsonPropertyName("minecraftGameVersionId")]
    public uint MinecraftGameVersionId { get; set; }

    [JsonPropertyName("forgeVersion")]
    public string ForgeVersion { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public ModLoaderType Type { get; set; }

    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("filename")]
    public string Filename { get; set; }

    [JsonPropertyName("installMethod")]
    public ModLoaderInstallMethod InstallMethod { get; set; }

    [JsonPropertyName("latest")]
    public bool Latest { get; set; }

    [JsonPropertyName("recommended")]
    public bool Recommended { get; set; }

    [JsonPropertyName("approved")]
    public bool Approved { get; set; }

    [JsonPropertyName("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonPropertyName("mavenVersionString")]
    public string MavenVersionString { get; set; }

    [JsonPropertyName("versionJson")]
    public string VersionJson { get; set; }

    [JsonPropertyName("librariesInstallLocation")]
    public string LibrariesInstallLocation { get; set; }

    [JsonPropertyName("minecraftVersion")]
    public string MinecraftVersion { get; set; }

    [JsonPropertyName("additionalFilesJson")]
    public string AdditionalFilesJson { get; set; }

    [JsonPropertyName("modLoaderGameVersionId")]
    public uint ModloaderGameVersionId { get; set; }

    [JsonPropertyName("modLoaderGameVersionTypeId")]
    public uint ModloaderGameVersionTypeId { get; set; }

    [JsonPropertyName("modLoaderGameVersionStatus")]
    public GameVersionStatus ModloaderGameVersionStatus { get; set; }

    [JsonPropertyName("modLoaderGameVersionTypeStatus")]
    public GameVersionTypeStatus ModloaderGameVersionTypeStatus { get; set; }

    [JsonPropertyName("mcGameVersionId")]
    public uint McGameVersionId { get; set; }

    [JsonPropertyName("mcGameVersionTypeId")]
    public uint McGameVersionTypeId { get; set; }

    [JsonPropertyName("mcGameVersionStatus")]
    public GameVersionStatus McGameVersionStatus { get; set; }

    [JsonPropertyName("mcGameVersionTypeStatus")]
    public GameVersionTypeStatus McGameVersionTypeStatus { get; set; }

    [JsonPropertyName("installProfileJson")]
    public string InstallProfileJson { get; set; }
}