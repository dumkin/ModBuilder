using CsQuery;
using ModBuilder.ProjectSystem;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;

namespace ModBuilder.Utilities
{
    public static class Parse
    {
        public delegate void CallbackGettingData(String ID);

        public static void AsyncGetAllData(String ID, CallbackGettingData Callback)
        {
            Thread ThreadAsyncGetAllData = new Thread(() => GetAllData(ID, Callback));
            ThreadAsyncGetAllData.Start();
        }
        public static void GetAllData(String ID, CallbackGettingData Callback)
        {
            WebClient Client = new WebClient();
            string DOM = Client.DownloadString("http://minecraft.curseforge.com/projects/" + ID + "/files");

            CQ Query = CQ.Create(DOM);

            GetName(ID, Query);
            GetType(ID, Query);
            GetImageURL(ID, Query);

            GetImage(ID);

            Callback(ID);
        }

        public static void GetName(String ID, CQ Query)
        {
            String Name = Query["span.overflow-tip"].Text();

            Project.Extension[ID].Name = Name;
        }

        public static void GetType(String ID, CQ Query)
        {
            String Type = Query["h2.RootGameCategory > a"].Text();

            Project.Extension[ID].Type = Type;
        }

        public static void GetImageURL(String ID, CQ Query)
        {
            String ImageURL = Query["a.e-avatar64"].Attr("href");

            Project.Extension[ID].ImageURL = ImageURL;
        }

        public static void GetImage(String ID)
        {
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(Project.Extension[ID].ImageURL);
            MemoryStream ms = new MemoryStream(bytes);
            Image Image = Image.FromStream(ms);

            Project.Extension[ID].Image = Image;
        }
    }
}