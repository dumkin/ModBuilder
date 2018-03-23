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

        [JsonProperty]
        public String ImageURL;

        public Image Image;

        public List<String> Versions = new List<String>();
    }
}