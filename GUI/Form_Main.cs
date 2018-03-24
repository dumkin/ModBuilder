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

            LoadCache();
            CheckCache();
        }
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        public void AddItem(String ID, String Name)
        {
            Project.List[Name] = ID;
            Build_List.Items.Add(Name);
        }
        public void LoadCache()
        {
            Enabled = false;

            foreach (var Item in Project.Extension)
            {
                AddItem(Item.Key, Item.Value.Name);
            }

            Enabled = true;
        }

        public void CheckCache()
        {
            foreach (var Item in Project.Extension)
            {
               Parse.AsyncGetAllData(Item.Key, CallbackCheckingCache);
            }
        }

        public void AddCache(String ID, String Name)
        {
            Project.Extension.Add(ID, new Extension { });

            AddItem(ID, Name);

            Parse.AsyncGetAllData(ID, CallbackCheckingCache);
        }

        public void CallbackCheckingCache(String ID)
        {
            Project.CountCheckedCache++;

            if (Project.CountCheckedCache == Project.Extension.Count)
            {
                Config.Save(Project, Projects.SelectedProjectFile);

                BeginInvoke(new MethodInvoker(delegate
                {
                    Dependencies_List.Items.Clear();
                    foreach (var Item in Project.Dependencies)
                    {
                        Dependencies_List.Items.Add(Item.Key);
                    }
                }));

                Parse.GenerateAvailableVersions();
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

                Selected_Available_List.Items.Clear();
                foreach (var Item in Project.Extension[ID].Versions)
                {
                    Selected_Available_List.Items.Add(Item);
                }
            }
        }

        private void Search_Find_Click(object sender, EventArgs e)
        {
            Search_List.Items.Clear();
            Parse.AsyncSearch(Search_Edit.Text, SearchCallback);
        }

        public void SearchCallback()
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                foreach (var Item in Project.Search)
                {
                    Search_List.Items.Add(Item.Key);
                }
            }));
        }

        private void Search_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*var index = listBox1.SelectedIndex;

            if (index >= 0)
            {
                MessageBox.Show(Project.Search[listBox1.SelectedItem.ToString()]);
            }*/
        }

        private void Search_Add_Click(object sender, EventArgs e)
        {
            AddCache(Project.Search[Search_List.SelectedItem.ToString()], Search_List.SelectedItem.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ID = Project.List[Build_List.SelectedItem.ToString()];

            Search_List.Items.Clear();
            //MessageBox.Show(Project.Extension[ID].Versions.ToString());
            foreach (var item in Project.Extension[ID].Versions)
            {
                Search_List.Items.Add(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Search_List.Items.Clear();
            foreach (var item in Project.AvailableVersions)
            {
                Search_List.Items.Add(item);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Download_FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Enabled = false;

                Project.CountDownload = 0;

                foreach (var Item in Project.Extension)
                {
                    Parse.AsyncDownload(Item.Key, Search_List.SelectedItem.ToString(), Download_FolderBrowserDialog.SelectedPath, DownloadCallback);
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

        private void Selected_Control_Delete_Click(object sender, EventArgs e)
        {
            Enabled = false;

            Project.Extension.Remove(Project.List[Build_List.SelectedItem.ToString()]);

            Config.Save(Project, Projects.SelectedProjectFile);

            Build_List.Items.Remove(Build_List.SelectedItem);
            Parse.GenerateAvailableVersions();

            Enabled = true;
        }

        
    }
}