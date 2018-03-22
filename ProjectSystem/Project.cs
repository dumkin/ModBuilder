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

        public static int CountCheckedCache = 0;
    }
}