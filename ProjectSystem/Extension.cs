using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ModBuilder.ProjectSystem
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Extension
    {
        [JsonProperty]
        public String Name;

        [JsonProperty]
        public String Type;

        public String ImageURL;

        public String FileName;

        public String FileURL;

        public Image Image;

        public List<String> Versions = new List<String>();

        public List<String> Dependencies = new List<String>();

        public List<String> Dependents = new List<String>();
    }
}