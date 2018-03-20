using System;
using System.Collections.Generic;

namespace ModBuilder.Project
{
    public class PProject
    {
        public static String SName;

        public static List<String> SExtension_ID = new List<String>();

        public static Dictionary<String, String> SExtension_Name = new Dictionary<String, String>();
        public static Dictionary<String, String> SExtension_Type = new Dictionary<String, String>();
        public static Dictionary<String, String> SExtension_ImageURL = new Dictionary<String, String>();

        public String Name;

        public List<String> Extension_ID = new List<String>();

        public Dictionary<String, String> Extension_Name = new Dictionary<String, String>();
        public Dictionary<String, String> Extension_Type = new Dictionary<String, String>();
        public Dictionary<String, String> Extension_ImageURL = new Dictionary<String, String>();

        public void ToExemplar()
        {
            Name = SName;
            Extension_ID = SExtension_ID;
            Extension_Name = SExtension_Name;
            Extension_Type = SExtension_Type;
            Extension_ImageURL = SExtension_ImageURL;
        }

        public void ToStatic()
        {
            SName = Name;
            SExtension_ID = Extension_ID;
            SExtension_Name = Extension_Name;
            SExtension_Type = Extension_Type;
            SExtension_ImageURL = Extension_ImageURL;
        }
    }
}