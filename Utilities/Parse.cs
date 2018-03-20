using CsQuery;
using ModBuilder.Project;
using System;
using System.Net;
using System.Threading;

namespace ModBuilder.Extension
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

            Callback(ID);
        }


        /*public static void AsyncGetName(String ID, CQ Query)
        {
            Thread ThreadAsyncGetName = new Thread(() => GetName(ID, Query));
            ThreadAsyncGetName.Start();
        }*/
        public static void GetName(String ID, CQ Query)
        {
            String Name = Query["span.overflow-tip"].Text();

            PProject.SExtension_Name[ID] = Name;
        }


        /*public static void AsyncGetType(String ID, CQ Query)
        {
            Thread ThreadAsyncGetType = new Thread(() => GetType(ID, Query));
            ThreadAsyncGetType.Start();
        }*/
        public static void GetType(String ID, CQ Query)
        {
            String Type = Query["h2.RootGameCategory > a"].Text();

            PProject.SExtension_Type[ID] = Type;
        }


        /*public static void AsyncGetImageURL(String ID, CQ Query)
        {
            Thread ThreadAsyncGetImageURL = new Thread(() => GetImageURL(ID, Query));
            ThreadAsyncGetImageURL.Start();
        }*/
        public static void GetImageURL(String ID, CQ Query)
        {
            String ImageURL = Query["a.e-avatar64"].Attr("href");

            PProject.SExtension_ImageURL[ID] = ImageURL;
        }
    }
}