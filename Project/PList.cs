using System;
using System.Collections.Generic;
using System.IO;

namespace ModBuilder.Project
{
    public class PList
    {
        public static String SelectedProjectFile;
        public List<String> Data = new List<String>();

        public void Repair()
        {
            List<String> NewData = new List<String>();

            foreach (var Item in Data)
            {
                if (File.Exists(Item))
                {
                    NewData.Add(Item);
                }
            }

            Data = NewData;
        }
    }
}