using CsQuery;
using ModBuilder.Project;
using System;
using System.Net;
using System.Threading;

namespace ModBuilder.Extension
{
    public static class Parse
    {
        public static void AsyncGetImageURL(String ID)
        {
            Thread ThreadAsyncGetImageURL = new Thread(() => GetImageURL(ID));
            ThreadAsyncGetImageURL.Start();
        }

        public static void GetImageURL(String ID)
        {
            WebClient Client = new WebClient();
            string DOM = Client.DownloadString("http://minecraft.curseforge.com/projects/" + ID + "/files");

            CQ Query = CQ.Create(DOM);

            String ImageURL = Query["a.e-avatar64"].Attr("href");

            PProject.SExtension_ImageURL[ID] = ImageURL;

            // PProject test = new PProject();
            // test.ToExemplar();
            // Project.Config.Save(test, PList.SelectedProjectFile);

            /*foreach (IDomObject obj in Query.Find("a.e-avatar64"))
                Console.WriteLine(obj.GetAttribute("href"));*/

            //explode('"', $event->sender->findFirst('a.e-avatar64')->html() )[1];
        }
    }
}