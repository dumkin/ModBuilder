using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ModBuilder.ProjectSystem
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Project
    {
        [JsonProperty]
        public static Dictionary<String, Extension> Extension = new Dictionary<String, Extension>();

        public static Dictionary<String, Extension> Dependencies = new Dictionary<String, Extension>();

        public static List<String> AvailableVersions = new List<String>();
        public static Dictionary<String, String> CodeVersions = new Dictionary<String, String>();

        public static String SelectedVersion;
        public static String DownloadFolder;
        public static String UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";

        public static Dictionary<String, String> List = new Dictionary<String, String>();
        public static Dictionary<String, String> Search = new Dictionary<String, String>();

        public static int CountCheckedCache = 0;
        public static int CountDownload = 0;
    }
}