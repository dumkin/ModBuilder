using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Curse.Integration.Models.Enums;
using Curse.Integration.Models.Mods;
using ModBuilder.Library.Classes;
using ModBuilder.Library.Extensions;
using File = Curse.Integration.Models.Files.File;

namespace ModBuilder.Windows;

public partial class MainWindow : Window
{
    private static string _selectedVersion;
    private static string _downloadFolder;
    private static int _countDownload;
    private static int _countTotal;

    public static Modpack Modpack;

    public MainWindow()
    {
        IsEnabled = false;

        InitializeComponent();

        var formProject = new ProjectWindow();
        do
        {
            formProject.ShowDialog();
        } while (Modpack == null);

        DataContext = this;
        Modpack.AddonsChanged += AddonsChanged;

        DrawModsList();
        GenerateAvailableVersions();

        IsEnabled = true;
    }

    public ObservableCollection<Mod> SearchList { get; } = new();
    public ObservableCollection<Mod> ModList { get; } = new();
    public ObservableCollection<Mod> DependencyList { get; } = new();
    public ObservableCollection<string> VersionList { get; } = new();
    public ObservableCollection<string> ModVersionList { get; } = new();
    public ObservableCollection<File> ModFileList { get; } = new();

    private async void AddonsChanged(object sender, EventArgs e)
    {
        try
        {
            DrawModsList();
            GenerateAvailableVersions();
            await GenerateDependency();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #region [ Dependency ]

    public async Task GenerateDependency()
    {
        var mods = Modpack.GetAddons();
        var modsAndFileId = mods
            .Select(m => (m.Id, m.LatestFiles.FirstOrDefault()?.Id))
            .Where(m => m.Item2 != null)
            .ToList();

        var deps = new List<Mod>();
        foreach (var modAndFileId in modsAndFileId)
        {
            var depsMods = await Modpack.GetFileDependency(modAndFileId.Item1, modAndFileId.Item2!.Value);

            deps.AddRange(depsMods);
        }

        var modsIds = mods.Select(d => d.Id).ToList();
        deps = deps.Where(d => !modsIds.Contains(d.Id)).ToList();
        deps = deps.DistinctBy(d => d.Id).ToList();

        Dispatcher?.Invoke(() =>
        {
            DependencyList.Clear();
            foreach (var dep in deps)
            {
                DependencyList.Add(dep);
            }
        });
    }

    #endregion

    #region [ Download ]

    private async void ControlModpackDownload_Click(object sender, EventArgs e)
    {
        IsEnabled = false;

        var version = ControlModpackList.SelectedItem.ToString();
        if (!Modpack._minecraftVersions.ContainsKey(version))
        {
            IsEnabled = true;
            return;
        }

        var downloadFolder = Path.Join(Directory.GetCurrentDirectory(), "build");
        if (Directory.Exists(downloadFolder))
        {
            Directory.Delete(downloadFolder, true);
        }

        Directory.CreateDirectory(downloadFolder);

        _countDownload = 0;
        _countTotal = 0;

        await Task.Factory.StartNew(async () =>
        {
            using var client = new HttpClient();

            var deps = new List<uint>(64);
            var modIdDownloaded = new List<uint>(256);

            async Task DownloadMod(uint modId, string? prefix = null)
            {
                try
                {
                    if (modIdDownloaded.Contains(modId))
                    {
                        return;
                    }

                    var files = await Modpack.ModFilesByVersion(modId, version);
                    if (!files.Any())
                    {
                        Console.WriteLine($"{modId} empty");
                        return;
                    }
                    var selectedFile = files.MaxBy(f => f.FileDate);
                    var path = Path.Join(downloadFolder, (prefix ?? "") + selectedFile.FileName);
                    var url = await Modpack.ModFileDownloadUrl(modId, selectedFile.Id);

                    var bytes = await client.GetByteArrayAsync(url);
                    await System.IO.File.WriteAllBytesAsync(path, bytes);
                    modIdDownloaded.Add(modId);

                    foreach (var dep in selectedFile.Dependencies.Where(d => d.RelationType == FileRelationType.RequiredDependency))
                    {
                        deps.Add(dep.ModId);
                    }

                    // _countDownload++;
                    DownloadCallback();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{modId} error: " + exception.Message);
                }
            }

            var mods = Modpack.GetAddons();
            _countTotal = mods.Count;
            foreach (var mod in mods)
            {
                await DownloadMod(mod.Id);
                // await Task.Delay(TimeSpan.FromSeconds(1));
            }

            do
            {
                var currentDeps = deps.Distinct().ToList();
                _countTotal += currentDeps.Count;
                deps.Clear();
                foreach (var dep in currentDeps)
                {
                    await DownloadMod(dep, $"deps_{dep}_");
                    // await Task.Delay(TimeSpan.FromSeconds(1));
                }
            } while (deps.Any());


            Dispatcher?.Invoke(() =>
            {
                IsEnabled = true;
            });
        });
    }

    public void DownloadCallback()
    {
        Dispatcher?.Invoke(() =>
        {
            Interlocked.Increment(ref _countDownload);
            // _countDownload++;

            ControlStatus.Text = _countDownload + "/" + _countTotal;
            //
            // if (_countDownload == Modpack.GetAddons().Count /*+ Dependencies.Count*/)
            //     // BeginInvoke(new MethodInvoker(delegate
            //     // {
            // {
            //     IsEnabled = true;
            // }

            // }));
        });
    }

    #endregion

    #region [ Versions ]

    public void GenerateAvailableVersions()
    {
        var availableVersions = Modpack.AvailableVersions();

        Dispatcher?.Invoke(() =>
        {
            VersionList.Clear();
            VersionList.Add("Not chosen");
            foreach (var version in availableVersions)
            {
                VersionList.Add(version);
            }

            ControlModpackList.SelectedIndex = 0;
        });
    }

    private void ControlModpackList_Changed(object sender, EventArgs e)
    {
        ControlModpackDownload.IsEnabled = ControlModpackList.SelectedIndex > 0;
    }

    #endregion

    #region [ Mods ]

    private void DrawModsList()
    {
        try
        {
            var mods = Modpack.GetAddonsSortedByName();

            Dispatcher?.Invoke(() =>
            {
                ModList.Clear();
                foreach (var mod in mods)
                {
                    ModList.Add(mod);
                }

                ControlAddons.SelectedIndex = mods.Any() ? 0 : -1;
            });
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ControlAddons_Changed(object sender, EventArgs e)
    {
        if (ControlAddons.SelectedIndex < 0)
        {
            ControlSelectedDelete.IsEnabled = false;
            return;
        }

        ControlSelectedDelete.IsEnabled = true;

        var mod = Modpack.GetAddonsSortedByName()[ControlAddons.SelectedIndex];

        ControlSelectedName.Text = mod.Name;
        ControlSelectedType.Text = mod.DownloadCount.ToString();

        var screenshotUrl = mod.GetDefaultScreenshot()?.Url;
        ControlSelectedImage.Source = !string.IsNullOrWhiteSpace(screenshotUrl) ? new BitmapImage(new Uri(screenshotUrl)) : null;

        DrawControlSelectedVersions(mod.Id);

        // var files = await Modpack.ModFiles(addon.Id);
        // var k = new List<File>(files);
        // SelectedFiles.ItemsSource = k;
        // Draw(this, EventArgs.Empty);
    }

    private async void ControlSelectedDelete_Click(object sender, EventArgs e)
    {
        try
        {
            IsEnabled = false;

            var mod = Modpack.GetAddonsSortedByName()[ControlAddons.SelectedIndex];

            Modpack.RemoveAddon(mod.Id);
            await Modpack.Save();

            Dispatcher?.Invoke(() =>
            {
                DrawModsList();

                IsEnabled = true;
            });
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DrawControlSelectedVersions(uint? modId = null)
    {
        if (modId == null)
        {
            Dispatcher?.Invoke(() => ModVersionList.Clear());
        }

        var mod = Modpack.GetAddons().FirstOrDefault(m => m.Id == modId);
        if (mod == null)
        {
            return;
        }

        var modVersions = mod.GetAvailableVersionsLabels(Modpack._minecraftVersions);

        Dispatcher?.Invoke(() =>
        {
            ModVersionList.Clear();
            foreach (var modVersion in modVersions)
            {
                ModVersionList.Add(modVersion);
            }
        });
    }

    #endregion

    #region [ Search ]

    private async void ControlSearchFind_Click(object sender, EventArgs e)
    {
        try
        {
            var query = ControlSearchQuery.Text;

            await Task.Factory.StartNew(async () =>
            {
                var addons = await Modpack.Search(query);

                Dispatcher?.Invoke(() =>
                {
                    SearchList.Clear();
                    foreach (var mod in addons)
                    {
                        SearchList.Add(mod);
                    }
                });
            });
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ControlSearchAdd_Click(object sender, EventArgs e)
    {
        var modId = SearchList[ControlSearchList.SelectedIndex].Id;

        await Task.Factory.StartNew(async () =>
        {
            await Modpack.AddAddon(modId);
            await Modpack.Save();
        });
    }

    private void ControlSearchList_Changed(object sender, EventArgs e)
    {
        ControlSearchAdd.IsEnabled = ControlSearchList.SelectedIndex >= 0;
    }

    #endregion
}