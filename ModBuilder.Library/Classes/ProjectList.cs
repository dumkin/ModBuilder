using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ModBuilder.Library.Classes;

public static class ProjectList
{
    private const string ProjectsFolder = "projects";
    private const string ModpackFileExtension = "modpack";

    private static readonly Mutex ProjectsMutex = new();
    private static List<string> _projects = new();

    static ProjectList()
    {
        PrepareProjectFolder();
    }

    private static void PrepareProjectFolder()
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), ProjectsFolder));
    }

    public static string ProjectFolder(string name)
    {
        PrepareProjectFolder();

        return Path.Combine(Directory.GetCurrentDirectory(), ProjectsFolder,
            Path.ChangeExtension(name, ModpackFileExtension));
    }

    public static void Load()
    {
        try
        {
            ProjectsMutex.WaitOne();

            PrepareProjectFolder();
            var projectsPath = Path.Combine(Directory.GetCurrentDirectory(), ProjectsFolder);
            var projectsPattern = $"*.{ModpackFileExtension}";

            var projects = Directory.GetFiles(projectsPath, projectsPattern, SearchOption.TopDirectoryOnly);

            _projects = projects.Select(Path.GetFileNameWithoutExtension).ToList();
        }
        finally
        {
            ProjectsMutex.ReleaseMutex();
        }
    }

    public static IEnumerable<string> Get()
    {
        try
        {
            ProjectsMutex.WaitOne();

            return _projects;
        }
        finally
        {
            ProjectsMutex.ReleaseMutex();
        }
    }

    public static async Task<Modpack> GetModpack(string name)
    {
        try
        {
            ProjectsMutex.WaitOne();

            if (!_projects.Contains(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            var modpack = new Modpack(name);
            await modpack.Load();

            return modpack;
        }
        finally
        {
            ProjectsMutex.ReleaseMutex();
        }
    }

    public static async Task Add(string name)
    {
        try
        {
            ProjectsMutex.WaitOne();

            if (_projects.Contains(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var modpack = new Modpack(name);
            await modpack.Save();

            _projects.Add(name);
        }
        finally
        {
            ProjectsMutex.ReleaseMutex();
        }
    }

    public static void Remove(string name)
    {
        try
        {
            ProjectsMutex.WaitOne();

            if (!_projects.Contains(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            _projects.Remove(name);

            var path = ProjectFolder(name);

            File.Delete(path);
        }
        finally
        {
            ProjectsMutex.ReleaseMutex();
        }
    }
}