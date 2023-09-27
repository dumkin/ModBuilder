using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Curse.Integration.Clients;
using Curse.Integration.Models.Mods;
using File = Curse.Integration.Models.Files.File;

namespace ModBuilder.Library.Classes;

public class CacheLayer
{
    private readonly string _cachePath;
    private readonly CurseClient _client;
    private readonly ConcurrentDictionary<(uint, uint), File> _files = new();
    private readonly ConcurrentDictionary<uint, Mod> _mods = new();

    public CacheLayer(CurseClient client)
    {
        _client = client;

        _cachePath = Path.Combine(Directory.GetCurrentDirectory(), "cache");

        if (!Directory.Exists(_cachePath))
        {
            Directory.CreateDirectory(_cachePath);
        }
    }

    public async ValueTask<Mod> GetMod(uint id)
    {
        return (await GetMods(new List<uint> { id })).First();
    }

    public async ValueTask<List<Mod>> GetMods(List<uint> ids)
    {
        var unknownMods = ids.Where(i => !_mods.ContainsKey(i)).ToList();
        if (unknownMods.Any())
        {
            var newMods = await _client.GetModsByIdListAsync(new GetModsByIdsListRequestBody
            {
                ModIds = unknownMods
            });
            if (newMods == null)
            {
                throw new ApplicationException();
            }

            foreach (var newMod in newMods.Data)
            {
                _mods.AddOrUpdate(newMod.Id, _ => newMod, (_, _) => newMod);
            }
        }

        return _mods
            .Where(m => ids.Contains(m.Key))
            .Select(m => m.Value)
            .ToList();
    }

    public async ValueTask<File> GetFile(uint modId, uint id)
    {
        if (_files.TryGetValue((modId, id), out var file))
        {
            return file;
        }

        var newFile = await _client.GetModFileAsync(modId, id);
        if (newFile == null)
        {
            throw new ApplicationException();
        }

        _files.AddOrUpdate((modId, id), _ => newFile.Data, (_, _) => newFile.Data);

        return newFile.Data;
    }

    public async ValueTask Load()
    {
        var modPaths = Directory.EnumerateFiles(_cachePath, "*.mod.cache");
        foreach (var modPath in modPaths)
        {
            var json = await System.IO.File.ReadAllTextAsync(modPath);
            var mod = JsonSerializer.Deserialize<Mod>(json);
            if (mod == null)
            {
                System.IO.File.Delete(modPath);
                continue;
            }

            _mods.AddOrUpdate(mod.Id, _ => mod, (_, _) => mod);
        }

        var filePaths = Directory.EnumerateFiles(_cachePath, "*.file.cache");
        foreach (var filePath in filePaths)
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var file = JsonSerializer.Deserialize<File>(json);
            if (file == null)
            {
                System.IO.File.Delete(filePath);
                continue;
            }

            _files.AddOrUpdate((file.ModId, file.Id), _ => file, (_, _) => file);
        }
    }

    public async Task Save()
    {
        foreach (var (modId, mod) in _mods)
        {
            var path = Path.Join(_cachePath, $"{modId}.mod.cache");

            var json = JsonSerializer.Serialize(mod);
            await System.IO.File.WriteAllTextAsync(path, json);
        }

        foreach (var ((modId, fileId), file) in _files)
        {
            var path = Path.Join(_cachePath, $"{modId}-{fileId}.file.cache");

            var json = JsonSerializer.Serialize(file);
            await System.IO.File.WriteAllTextAsync(path, json);
        }
    }
}