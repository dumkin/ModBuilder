using System.Collections.Generic;
using System.Linq;
using Curse.Integration.Models.Mods;

namespace ModBuilder.Library.Extensions;

public static class AddonExtensions
{
    public static List<string> GetAvailableVersionsLabels(this Mod mod, Dictionary<string, string> minecraftVersions)
    {
        if (mod?.LatestFiles == null)
        {
            return new List<string>();
        }

        // minecraftVersions.

        var result = mod.LatestFilesIndexes
            .DistinctBy(f => f.GameVersion)
            .Where(f => !string.IsNullOrWhiteSpace(f.GameVersion))
            .Select(f => (f.GameVersion, Padded: minecraftVersions[f.GameVersion]))
            .OrderByDescending(f => f.Padded)
            .Select(f => f.GameVersion)
            .ToList();

        // result.Sort(new SameVersionComparer());
        // result.Reverse();

        return result;
    }

    public static ModAsset? GetDefaultScreenshot(this Mod addon)
    {
        if (addon?.Screenshots == null || addon.Screenshots.Count == 0)
        {
            return null;
        }

        var result = addon.Screenshots.MinBy(p => p.Id);

        return result ?? addon.Screenshots.First();
    }
}