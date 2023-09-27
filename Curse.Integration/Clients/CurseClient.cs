using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Curse.Integration.Models;
using Curse.Integration.Models.Enums;
using Curse.Integration.Models.Files;
using Curse.Integration.Models.Games;
using Curse.Integration.Models.Minecraft;
using Curse.Integration.Models.Mods;
using Microsoft.AspNetCore.Http.Extensions;

namespace Curse.Integration.Clients;

public class CurseClient
{
    private const string BaseAddress = "https://api.curseforge.com";

    private readonly HttpClient _client;

    public CurseClient(HttpClient client, string apiKey)
    {
        _client = client;

        _client.BaseAddress = new Uri(BaseAddress);
        _client.DefaultRequestHeaders.Add("x-api-key", apiKey);
    }

    public async Task<GenericListResponse<Category>?> GetCategoriesAsync(
        uint? gameId = null,
        uint? classId = null,
        bool? classesOnly = null
    )
    {
        var query = BuildQuery(new Dictionary<string, string?>
        {
            { "gameId", gameId?.ToString() },
            { "classId", classId?.ToString() },
            { "classesOnly", classesOnly?.ToString() }
        });

        Console.WriteLine("/v1/categories" + query);

        return await _client.GetFromJsonAsync<GenericListResponse<Category>>("/v1/categories" + query);
    }

    public async Task<GenericListResponse<Mod>?> SearchModsAsync(
        uint gameId,
        uint? classId = null,
        uint? categoryId = null,
        string? gameVersion = null,
        string? searchFilter = null,
        ModsSearchSortField? sortField = null,
        SortOrder sortOrder = SortOrder.Descending,
        ModLoaderType? modLoaderType = null,
        string? slug = null,
        uint? gameVersionTypeId = null,
        uint? index = null,
        uint? pageSize = null
    )
    {
        var query = BuildQuery(new Dictionary<string, string?>
        {
            { "gameId", gameId.ToString() },
            { "classId", classId?.ToString() },
            { "categoryId", categoryId?.ToString() },
            { "gameVersion", gameVersion },
            { "searchFilter", searchFilter },
            { "slug", slug },
            { "sortField", sortField?.ToString() },
            { "sortOrder", sortOrder == SortOrder.Descending ? "desc" : "asc" },
            { "modLoaderType", modLoaderType?.ToString() },
            { "gameVersionTypeId", gameVersionTypeId?.ToString() },
            { "index", index?.ToString() },
            { "pageSize", pageSize?.ToString() }
        });

        Console.WriteLine("/v1/mods/search" + query);

        return await _client.GetFromJsonAsync<GenericListResponse<Mod>>("/v1/mods/search" + query);
    }

    public async Task<GenericResponse<Mod>?> GetModAsync(uint modId)
    {
        Console.WriteLine($"/v1/mods/{modId}");
        return await _client.GetFromJsonAsync<GenericResponse<Mod>>($"/v1/mods/{modId}");
    }

    public async Task<GenericResponse<string>?> GetModDescriptionAsync(uint modId)
    {
        Console.WriteLine($"/v1/mods/{modId}/description");
        return await _client.GetFromJsonAsync<GenericResponse<string>>($"/v1/mods/{modId}/description");
    }

    public async Task<GenericListResponse<Mod>?> GetModsByIdListAsync(GetModsByIdsListRequestBody body)
    {
        Console.WriteLine("/v1/mods " + string.Join(',', body.ModIds));
        var response = await _client.PostAsJsonAsync("/v1/mods", body);

        return await response.Content.ReadFromJsonAsync<GenericListResponse<Mod>>();
    }

    public async Task<GenericResponse<FeaturedModsResponse>?> GetFeaturedModsAsync(GetFeaturedModsRequestBody body)
    {
        var response = await _client.PostAsJsonAsync("/v1/mods/featured", body);

        return await response.Content.ReadFromJsonAsync<GenericResponse<FeaturedModsResponse>>();
    }

    public async Task<GenericListResponse<Game>?> GetGamesAsync(uint? index = null, uint? pageSize = null)
    {
        var query = BuildQuery(new Dictionary<string, string?>
        {
            { "index", index?.ToString() },
            { "pageSize", pageSize?.ToString() }
        });

        return await _client.GetFromJsonAsync<GenericListResponse<Game>>("/v1/games" + query);
    }

    public async Task<GenericResponse<Game>?> GetGameAsync(uint gameId)
    {
        return await _client.GetFromJsonAsync<GenericResponse<Game>>($"/v1/games/{gameId}");
    }

    public async Task<GenericListResponse<GameVersionsByType>?> GetGameVersionsAsync(uint gameId)
    {
        return await _client.GetFromJsonAsync<GenericListResponse<GameVersionsByType>>($"/v1/games/{gameId}/versions");
    }

    public async Task<GenericListResponse<GameVersionType>?> GetGameVersionTypesAsync(uint gameId)
    {
        return await _client.GetFromJsonAsync<GenericListResponse<GameVersionType>>($"/v1/games/{gameId}/version-types");
    }

    public async Task<GenericResponse<File>?> GetModFileAsync(uint modId, uint fileId)
    {
        Console.WriteLine($"/v1/mods/{modId}/files/{fileId}");
        return await _client.GetFromJsonAsync<GenericResponse<File>>($"/v1/mods/{modId}/files/{fileId}");
    }

    public async Task<GenericListResponse<File>?> GetModFilesAsync(
        uint modId,
        string? gameVersion = null,
        ModLoaderType? modLoaderType = null,
        string? gameVersionFlavor = null,
        uint? index = null,
        uint? pageSize = null
    )
    {
        var query = BuildQuery(new Dictionary<string, string?>
        {
            { "gameVersion", gameVersion },
            { "modLoaderType", modLoaderType?.ToString() },
            { "gameVersionFlavor", gameVersionFlavor },
            { "index", index?.ToString() },
            { "pageSize", pageSize?.ToString() }
        });

        Console.WriteLine($"/v1/mods/{modId}/files" + query);

        return await _client.GetFromJsonAsync<GenericListResponse<File>>($"/v1/mods/{modId}/files" + query);
    }

    public async Task<GenericListResponse<File>?> GetFilesAsync(GetModFilesRequestBody body)
    {
        var response = await _client.PostAsJsonAsync("/v1/mods/files", body);

        return await response.Content.ReadFromJsonAsync<GenericListResponse<File>>();
    }

    public async Task<GenericResponse<string>?> GetModFileChangelogAsync(uint modId, uint fileId)
    {
        return await _client.GetFromJsonAsync<GenericResponse<string>>($"/v1/mods/{modId}/files/{fileId}/changelog");
    }

    public async Task<GenericResponse<string>?> GetModFileDownloadUrlAsync(uint modId, uint fileId)
    {
        Console.WriteLine($"/v1/mods/{modId}/files/{fileId}/download-url");

        return await _client.GetFromJsonAsync<GenericResponse<string>>($"/v1/mods/{modId}/files/{fileId}/download-url");
    }

    public string BuildQuery(Dictionary<string, string?> parameters)
    {
        return new QueryBuilder(parameters
            .Where(p => string.IsNullOrEmpty(p.Key) == false && string.IsNullOrEmpty(p.Value) == false)
            .ToDictionary(k => k.Key, k => k.Value!)
        ).ToQueryString().Value ?? "";
    }

    public async Task<GenericListResponse<MinecraftGameVersion>?> GetMinecraftVersions()
    {
        Console.WriteLine("/v1/minecraft/version");
        return await _client.GetFromJsonAsync<GenericListResponse<MinecraftGameVersion>>("/v1/minecraft/version");
    }

    public async Task<GenericResponse<MinecraftGameVersion>?> GetSpecificMinecraftVersionInfo(string gameVersionString)
    {
        return await _client.GetFromJsonAsync<GenericResponse<MinecraftGameVersion>>($"/v1/minecraft/version/{gameVersionString}");
    }

    public async Task<GenericListResponse<MinecraftModLoaderIndex>?> GetMinecraftModloaders(
        string? version = null,
        bool includeAll = false
    )
    {
        var query = BuildQuery(new Dictionary<string, string?>
        {
            { "version", version },
            { "includeAll", includeAll.ToString() }
        });

        Console.WriteLine("/v1/mods/search" + query);

        return await _client.GetFromJsonAsync<GenericListResponse<MinecraftModLoaderIndex>>("/v1/minecraft/modloader" + query);
    }

    public async Task<GenericResponse<MinecraftModloaderInfo>?> GetSpecificMinecraftModloaderInfo(string modloaderName)
    {
        return await _client.GetFromJsonAsync<GenericResponse<MinecraftModloaderInfo>>($"/v1/minecraft/modloader/{modloaderName}");
    }
}