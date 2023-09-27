using System.Collections.Generic;

namespace Curse.Integration.Models.Mods;

public class FeaturedModsResponse
{
    public List<Mod> Featured { get; set; } = new();
    public List<Mod> Popular { get; set; } = new();
    public List<Mod> RecentlyUpdated { get; set; } = new();
}