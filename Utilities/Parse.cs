using CsQuery;
using ModBuilder.ProjectSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace ModBuilder.Utilities
{
    public static class Parse
    {
        public delegate void CallbackGettingData(String ID);
        public static void AsyncGetAllData(String ID, CallbackGettingData Callback)
        {
            var ThreadAsyncGetAllData = new Thread(() => GetAllData(ID, Callback));
            ThreadAsyncGetAllData.Start();
        }
        public static void GetAllData(String ID, CallbackGettingData Callback)
        {
            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            var DOM = Client.DownloadString("http://minecraft.curseforge.com/projects/" + ID + "/files");

            var Query = CQ.Create(DOM);

            GetName(ID, Query);
            GetType(ID, Query);
            GetVersions(ID, Query);
            GetImageURL(ID, Query);

            GetImage(ID);

            GetDependencies(ID);

            Callback(ID);
        }

        public static void GetName(String ID, CQ Query)
        {
            var Name = Query["span.overflow-tip"].Text();

            Project.Extension[ID].Name = Name;
        }

        public static void GetType(String ID, CQ Query)
        {
            var Type = Query["h2.RootGameCategory > a"].Text();

            Project.Extension[ID].Type = Type;
        }

        public static void GetVersions(String ID, CQ Query)
        {
            var Options = Query["select#filter-game-version > option"];

            foreach (var Item in Options)
            {
                var FindAll = new Regex("All");
                var FindMinecraft = new Regex("Minecraft");

                if (FindAll.IsMatch(Item.TextContent) || FindMinecraft.IsMatch(Item.TextContent))
                {
                    continue;
                }

                var FindJava = new Regex("Java");

                if (FindJava.IsMatch(Item.TextContent))
                {
                    break;
                }

                var OnlyVersion = new Regex("[^0-9.]");
                var Text = OnlyVersion.Replace(Item.TextContent, "");

                Project.Extension[ID].Versions.Add(Text);
                Project.CodeVersions[Text] = Item.GetAttribute("value");
            }
        }

        public static void GetImageURL(String ID, CQ Query)
        {
            var ImageURL = Query["a.e-avatar64"].Attr("href");

            Project.Extension[ID].ImageURL = ImageURL;
        }

        public static void GetImage(String ID)
        {
            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            var bytes = Client.DownloadData(Project.Extension[ID].ImageURL);
            var ms = new MemoryStream(bytes);
            var ImageSource = Image.FromStream(ms);

            Project.Extension[ID].Image = ImageSource;
        }

        public static void GetDependencies(String ID)
        {
            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            var DOM = Client.DownloadString("https://minecraft.curseforge.com/projects/" + ID + "/relations/dependencies?filter-related-dependencies=3");

            var Query = CQ.Create(DOM)["div.name-wrapper.overflow-tip > a"];

            foreach (var Item in Query)
            {
                var URL = Item.GetAttribute("href");

                var ExtensionID = URL.Split('/')[4];
                var ExtensionName = Item.TextContent;

                Project.Extension[ID].Dependencies.Add(ExtensionID);

                if (Project.Extension.ContainsKey(ExtensionID))
                {
                    continue;
                }

                if (!Project.Dependencies.ContainsKey(ExtensionID))
                {
                    Project.Dependencies[ExtensionID] = new Extension
                    {
                        Name = ExtensionName,
                        Dependents = new List<String> { ID }
                    };
                }
                else
                {
                    Project.Dependencies[ExtensionID].Dependents.Add(ID);
                }
            }
        }

        public delegate void CallbackSearch();
        public static void AsyncSearch(String Line, CallbackSearch Callback, int Page = 0)
        {
            var ThreadAsyncSearch = new Thread(() => Search(Line, Callback, Page));
            ThreadAsyncSearch.Start();
        }
        public static void Search(String Line, CallbackSearch Callback, int Page)
        {
            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            var DOM = Client.DownloadString("https://minecraft.curseforge.com/search?projects-page=" + Page.ToString() + "&search=" + Line);

            var Query = CQ.Create(DOM)["div.results-name > a"];

            Project.Search.Clear();

            foreach (var Item in Query)
            {
                var URL = Item.GetAttribute("href");
                var ID = URL.Split('/')[2].Split('?')[0];
                
                Project.Search[Item.TextContent] = ID;
            }

            Callback();
        }

        public static void GenerateAvailableVersions()
        {
            Project.AvailableVersions.Clear();
            var Cleared = true;

            foreach (var Item in Project.Extension)
            {
                if (Cleared)
                {
                    Project.AvailableVersions = Item.Value.Versions;

                    Cleared = false;
                }
                else
                {
                    AddAvailableVersions(Item.Key);
                }
            }
        }
        public static void AddAvailableVersions(String ID)
        {
            Project.AvailableVersions = Project.AvailableVersions.Intersect(Project.Extension[ID].Versions).ToList();
        }

        public delegate void CallbackDownload(String ID);
        public static void AsyncDownload(String ID, String Type, CallbackDownload Callback)
        {
            var ThreadAsyncDownload = new Thread(() => Download(ID, Type, Callback));
            ThreadAsyncDownload.Start();
        }
        public static void Download(String ID, String Type, CallbackDownload Callback)
        {
            PrepareDownload(ID, Type);

            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            Extension Ext;
            if (Type == "Extension")
            {
                Ext = Project.Extension[ID];
            }
            else
            {
                Ext = Project.Dependencies[ID];
            }

            Client.DownloadFile(Ext.FileURL, Project.DownloadFolder + "\\" + Ext.FileName);

            Callback(ID);
        }

        public static void PrepareDownload(String ID, String Type)
        {
            var Client = new WebClient();
            Client.Headers["UserAgent"] = Project.UserAgent;

            var FileListDOM = Client.DownloadString("http://minecraft.curseforge.com/projects/" + ID + "/files?filter-game-version=" + Project.CodeVersions[Project.SelectedVersion]);
            var FileListQuery = CQ.Create(FileListDOM);

            var URL = "http://minecraft.curseforge.com" + FileListQuery["a.overflow-tip"].Attr("href");

            var FileDOM = Client.DownloadString(URL);
            var FileQuery = CQ.Create(FileDOM);

            GetFileName(ID, Type, FileQuery);
            GetFileURL(ID, Type, URL);
        }

        public static void GetFileName(String ID, String Type, CQ Query)
        {
            var FileName = Query["div.info-data"].First().Text();

            if (Type == "Extension")
            {
                Project.Extension[ID].FileName = FileName;
            }
            else
            {
                Project.Dependencies[ID].FileName = FileName;
            }
        }
        public static void GetFileURL(String ID, String Type, String URL)
        {
            var ExplodeURL = URL.Split('/');

            var FirstCode = Int32.Parse(ExplodeURL[6].Substring(0, 4));
            var SecondCode = Int32.Parse(ExplodeURL[6].Substring(4));

            if (Type == "Extension")
            {
                Project.Extension[ID].FileURL = "https://media.forgecdn.net/files/" + FirstCode + "/" + SecondCode + "/" + Uri.EscapeDataString(Project.Extension[ID].FileName);
            }
            else
            {
                Project.Dependencies[ID].FileURL = "https://media.forgecdn.net/files/" + FirstCode + "/" + SecondCode + "/" + Uri.EscapeDataString(Project.Dependencies[ID].FileName);
            }
        }
    }
}