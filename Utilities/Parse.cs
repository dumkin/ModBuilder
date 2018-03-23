using CsQuery;
using ModBuilder.ProjectSystem;
using System;
using System.Drawing;
using System.IO;
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
            var DOM = Client.DownloadString("http://minecraft.curseforge.com/projects/" + ID + "/files");

            var Query = CQ.Create(DOM);

            GetName(ID, Query);
            GetType(ID, Query);
            GetVersions(ID, Query);
            GetImageURL(ID, Query);

            GetImage(ID);

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
            var wc = new WebClient();
            var bytes = wc.DownloadData(Project.Extension[ID].ImageURL);
            var ms = new MemoryStream(bytes);
            var ImageSource = Image.FromStream(ms);

            Project.Extension[ID].Image = ImageSource;
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
    }
}