using ModBuilder.ProjectSystem;
using ModBuilder.Utilities;
using System;
using System.IO;
using System.Windows.Forms;

namespace ModBuilder.GUI
{
    public partial class Form_Main : Form
    {
        Project Project;

        public Form_Main()
        {
            InitializeComponent();
            Enabled = false;

            var Form_Project = new Form_Project();
            Form_Project.ShowDialog();

            if (!File.Exists(Projects.SelectedProjectFile))
            {
                Environment.Exit(0);
            }

            Project = Config.Load<Project>(Projects.SelectedProjectFile);

            /*
            Project.Extension.Add("actually-additions", new Extension { });
            Project.Extension.Add("ambientsounds", new Extension { });
            Project.Extension.Add("applied-energistics-2", new Extension { });
            Project.Extension.Add("chunk-animator", new Extension { });
            Project.Extension.Add("deep-resonance", new Extension { });
            Project.Extension.Add("fast-leaf-decay", new Extension { });
            Project.Extension.Add("immersive-engineering", new Extension { });
            Project.Extension.Add("industrial-craft", new Extension { });
            Project.Extension.Add("itemphysic", new Extension { });
            Project.Extension.Add("journeymap", new Extension { });
            Project.Extension.Add("jei", new Extension { });
            Project.Extension.Add("just-enough-resources-jer", new Extension { });
            Project.Extension.Add("malisisdoors", new Extension { });
            Project.Extension.Add("minecolonies", new Extension { });
            Project.Extension.Add("more-overlays", new Extension { });
            Project.Extension.Add("mouse-tweaks", new Extension { });
            Project.Extension.Add("platforms", new Extension { });
            Project.Extension.Add("storage-drawers", new Extension { });
            Project.Extension.Add("the-one-probe", new Extension { });
            Project.Extension.Add("tinkers-construct", new Extension { });
            */

            foreach (var Item in Project.Extension)
            {
                Project.List[Item.Value.Name] = Item.Key;
            }

            foreach (var Item in Project.Extension)
            {
                Parse.AsyncGetAllData(Item.Key, CallbackCheckingCache);
            }

            Draw_Control_Available();
            Draw_Build_List();

            Enabled = true;
        }
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        public void CallbackCheckingCache(String ID)
        {
            Project.CountCheckedCache++;

            if (Project.CountCheckedCache == Project.Extension.Count)
            {
                Config.Save(Project, Projects.SelectedProjectFile);

                Parse.GenerateAvailableVersions();

                BeginInvoke(new MethodInvoker(delegate
                {
                    Draw_Build_List();
                    Draw_Dependencies_List();
                    Draw_Control_Available();
                }));
            }
        }

        private void Build_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Build_List.SelectedIndex >= 0)
            {
                var ID = Project.List[Build_List.SelectedItem.ToString()];

                Selected_Name.Text = Project.Extension[ID].Name;
                Selected_Type.Text = Project.Extension[ID].Type;
                Selected_Image.Image = Project.Extension[ID].Image;

                Draw_Selected_Available_List(ID);
            }
        }
        private void Selected_Control_Delete_Click(object sender, EventArgs e)
        {
            Enabled = false;

            Project.Extension.Remove(Project.List[Build_List.SelectedItem.ToString()]);

            Config.Save(Project, Projects.SelectedProjectFile);

            Parse.GenerateAvailableVersions();

            Draw_Build_List();
            Draw_Control_Available();

            Enabled = true;
        }

        private void Search_Find_Click(object sender, EventArgs e)
        {
            Search_Find.Enabled = false;

            Parse.AsyncSearch(Search_Edit.Text, SearchCallback);
        }
        private void Search_Add_Click(object sender, EventArgs e)
        {
            var Name = Search_List.SelectedItem.ToString();
            var ID = Project.Search[Name];

            Project.Extension.Add(ID, new Extension { Name = Name });
            Project.List[Name] = ID;

            Draw_Build_List();

            Parse.AsyncGetAllData(ID, CallbackCheckingCache);
        }
        public void SearchCallback()
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                Draw_Search_List();

                Search_Find.Enabled = true;
            }));
        }

        private void Control_Download_Click(object sender, EventArgs e)
        {
            if (Download_FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;

                Project.CountDownload = 0;

                foreach (var Item in Project.Extension)
                {
                    Parse.AsyncDownload(Item.Key, Control_Available.SelectedItem.ToString(), Download_FolderBrowserDialog.SelectedPath, DownloadCallback);
                }
            }
        }
        public void DownloadCallback(String ID)
        {
            Project.CountDownload++;

            if (Project.CountDownload == Project.Extension.Count)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Enabled = true;
                }));
            }
        }

        public void Draw_Control_Available()
        {
            Control_Available.Items.Clear();
            Control_Available.Items.Add("Not chosen");
            Control_Available.SelectedIndex = 0;

            foreach (var Item in Project.AvailableVersions)
            {
                Control_Available.Items.Add(Item);
            }
        }
        public void Draw_Search_List()
        {
            Search_List.Items.Clear();

            foreach (var Item in Project.Search)
            {
                Search_List.Items.Add(Item.Key);
            }
        }
        public void Draw_Build_List()
        {
            Build_List.Items.Clear();

            foreach (var Item in Project.Extension)
            {
                Build_List.Items.Add(Item.Value.Name);
            }
        }
        public void Draw_Dependencies_List()
        {
            Dependencies_List.Items.Clear();

            foreach (var Item in Project.Dependencies)
            {
                Dependencies_List.Items.Add(Item.Value.Name);
            }
        }
        public void Draw_Selected_Available_List(String ID)
        {
            Selected_Available_List.Items.Clear();

            foreach (var Item in Project.Extension[ID].Versions)
            {
                Selected_Available_List.Items.Add(Item);
            }
        }
    }
}