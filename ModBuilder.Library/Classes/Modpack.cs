using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Curse.Integration.Clients;
using Curse.Integration.Models.Enums;
using Curse.Integration.Models.Files;
using Curse.Integration.Models.Mods;
using ModBuilder.Library.Extensions;

namespace ModBuilder.Library.Classes;

public sealed class Modpack
{
    private const uint GameId = 432;

    private readonly List<Mod> _addons = new();
    private readonly Semaphore _addonsSafety;
    private readonly CacheLayer _cacheLayer;
    private readonly CurseClient _client;
    private readonly Dictionary<(uint, uint), List<Mod>> _fileDeps = new();
    private readonly Dictionary<uint, List<File>> _modFiles = new();
    private readonly string _name;
    public Dictionary<string, string> _minecraftVersions = new();

    public Modpack(string name)
    {
        _client = new CurseClient(new HttpClient(), "");
        _name = name;
        _cacheLayer = new CacheLayer(_client);
        _addonsSafety = new Semaphore(1, 1);
    }

    public event EventHandler AddonsChanged;

    private void OnAddonsChanged(EventArgs e)
    {
        AddonsChanged?.Invoke(this, e);
    }

    public IReadOnlyList<Mod> GetAddons()
    {
        try
        {
            _addonsSafety.WaitOne();

            return _addons.AsReadOnly();
        }
        finally
        {
            _addonsSafety.Release();
        }
    }

    public IReadOnlyList<Mod> GetAddonsSortedByName()
    {
        try
        {
            _addonsSafety.WaitOne();

            return _addons.OrderBy(a => a.Name).ToList().AsReadOnly();
        }
        finally
        {
            _addonsSafety.Release();
        }
    }

    public Task AddAddon(uint id)
    {
        return AddAddons(new[] { id });
    }

    public async Task<List<Mod>> Search(string query)
    {
        return (await _client.SearchModsAsync(GameId, 6, searchFilter: query, sortField: ModsSearchSortField.Featured)).Data;
    }

    public async Task<List<File>> ModFiles(uint modId)
    {
        return (await _client.GetModFilesAsync(modId)).Data;
    }

    public async Task<List<File>> ModFilesByVersion(uint modId, string version)
    {
        return (await _client.GetModFilesAsync(modId, version, ModLoaderType.Forge)).Data;
    }

    public async Task<string> ModFileDownloadUrl(uint modId, uint fileId)
    {
        return (await _client.GetModFileDownloadUrlAsync(modId, fileId)).Data;
    }

    public async Task AddAddons(IEnumerable<uint> ids)
    {
        try
        {
            _addonsSafety.WaitOne();

            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var idsList = ids.Where(id => _addons.All(addon => addon.Id != id)).ToList();
            if (idsList.Count == 0)
            {
                return;
            }

            var newMods = await _cacheLayer.GetMods(idsList);
            // var newMods = await _client.GetModsByIdListAsync(new GetModsByIdsListRequestBody
            // {
            // ModIds = idsList
            // });

            // foreach (var mod in newMods.Data)
            // {
            //     if (!_modFiles.ContainsKey(mod.Id))
            //     {
            //         _modFiles[mod.Id] = new List<Curse.Integration.Models.Files.File>();
            //     }
            //     _modFiles[mod.Id].Clear();
            //
            //     long maxFiles = 0;
            //     long processedFiles = 0;
            //     uint index = 0;
            //     do
            //     {
            //         var files = await _client.GetModFilesAsync(mod.Id, index: index);
            //         _modFiles[mod.Id].AddRange(files.Data);
            //         maxFiles = files.Pagination.TotalCount;
            //         processedFiles += files.Pagination.ResultCount;
            //         index++;
            //     } while (processedFiles < maxFiles);
            // }

            _addons.AddRange(newMods);
        }
        finally
        {
            _addonsSafety.Release();

            OnAddonsChanged(EventArgs.Empty);
        }
    }

    public void RemoveAddon(uint id)
    {
        try
        {
            _addonsSafety.WaitOne();

            _addons.RemoveAll(a => a.Id == id);
        }
        finally
        {
            _addonsSafety.Release();

            OnAddonsChanged(EventArgs.Empty);
        }
    }

    public async Task<List<Mod>> GetFileDependency(uint modId, uint fileId)
    {
        try
        {
            _addonsSafety.WaitOne();

            if (_fileDeps.TryGetValue((modId, fileId), out var dependency))
            {
                return dependency;
            }

            _fileDeps[(modId, fileId)] = new List<Mod>();
            var file = await _cacheLayer.GetFile(modId, fileId);

            var depsIds = file.Dependencies
                .Where(d => d.RelationType == FileRelationType.RequiredDependency)
                .Select(d => d.ModId).ToList();
            if (!depsIds.Any())
            {
                return _fileDeps[(modId, fileId)];
            }

            var mods = await _cacheLayer.GetMods(depsIds);

            _fileDeps[(modId, fileId)].AddRange(mods);

            return _fileDeps[(modId, fileId)];
        }
        finally
        {
            _addonsSafety.Release();
        }
    }

    public List<string> AvailableVersions()
    {
        try
        {
            _addonsSafety.WaitOne();

            var result = new List<string>();

            if (_addons.Count == 0)
            {
                return result;
            }

            result = _addons.First().GetAvailableVersionsLabels(_minecraftVersions);

            return _addons.Aggregate(result,
                (current, addon) => current.Intersect(addon.GetAvailableVersionsLabels(_minecraftVersions)).ToList());
        }
        finally
        {
            _addonsSafety.Release();
        }
    }

    private async Task GetAvailableVersions()
    {
        try
        {
            _addonsSafety.WaitOne();

            var versions = await _client.GetMinecraftVersions();

            string PadVersion(string version)
            {
                return string.Join('.', version.Split('.').Select(v => v.PadLeft(8, '0')));
            }

            if (versions != null)
            {
                _minecraftVersions = versions.Data
                    .Select(v => (v.VersionString, Padded: PadVersion(v.VersionString)))
                    .ToDictionary(v => v.VersionString, v => v.Padded);
            }
        }
        finally
        {
            _addonsSafety.Release();
        }
    }

    public async Task Load()
    {
        var path = ProjectList.ProjectFolder(_name);
        if (!System.IO.File.Exists(path))
        {
            throw new ArgumentException(nameof(_name));
        }

        var json = await System.IO.File.ReadAllTextAsync(path);

        var addons = JsonSerializer.Deserialize<List<uint>>(json) ?? new List<uint>();

        await GetAvailableVersions();

        await _cacheLayer.Load();

#pragma warning disable 4014
        AddAddons(addons);
#pragma warning restore 4014
    }

    public async Task Save()
    {
        try
        {
            _addonsSafety.WaitOne();

            var path = ProjectList.ProjectFolder(_name);

            var data = _addons.Select(a => a.Id).ToArray();

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await System.IO.File.WriteAllTextAsync(path, json);

            await _cacheLayer.Save();
        }
        finally
        {
            _addonsSafety.Release();
        }
    }
}