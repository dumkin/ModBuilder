using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModBuilder.ProjectSystem
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Projects
    {
        public static String SelectedProjectFile;

        [JsonProperty]
        public List<String> Files = new List<String>();

        public bool Valid()
        {
            foreach (var Item in Files)
            {
                if (!File.Exists(Item))
                {
                    return false;
                }
            }

            return true;
        }
        public void Repair()
        {
            List<String> Repaired = new List<String>();

            foreach (var Item in Files)
            {
                if (File.Exists(Item))
                {
                    Repaired.Add(Item);
                }
            }

            Files = Repaired;
        }
    }
}