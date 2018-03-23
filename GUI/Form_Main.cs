using ModBuilder.ProjectSystem;
using ModBuilder.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void LoadCache()
        {
            Enabled = false;

            foreach (var Item in Project.Extension)
            {
                AddItem(Item.Key, Item.Value.Name);
            }

            Enabled = true;
        }

        public void AddItem(String ID, String Name)
        {
            Project.List[Name] = ID;
            ListBox_Main.Items.Add(Name);
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

                GenerateAvailableVersions();
            }
        }

        private void ListBox_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_Main.SelectedIndex >= 0)
            {
                var ID = Project.List[ListBox_Main.SelectedItem.ToString()];

                PictureBox_Main.Image = Project.Extension[ID].Image;
                Label_Type.Text = Project.Extension[ID].Type;
            }
        }

        public void GenerateAvailableVersions()
        {
            Project.AvailableVersions.Clear();
            var Cleared = true;

            foreach (var Item in Project.Extension)
            {
                if (Cleared)
                {
                    Project.AvailableVersions = Item.Value.Versions;

                    Cleared = false;
                }
                else
                {
                    AddAvailableVersions(Item.Key);
                }
            }
        }

        public void AddAvailableVersions(String ID)
        {
            Project.AvailableVersions = Project.AvailableVersions.Intersect(Project.Extension[ID].Versions).ToList();
        }

        /*
         if (e.Button == MouseButtons.Right && ListView_Main.SelectedIndices.Count > 0)
            {
                if (ListView_Main.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    ContextMenuStrip_Main.Show(Cursor.Position);
                }
            }
        */

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Parse.AsyncSearch(textBox1.Text, SearchCallback);
        }

        public void SearchCallback()
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                foreach (var Item in Project.Search)
                {
                    listBox1.Items.Add(Item.Key);
                }
            }));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*var index = listBox1.SelectedIndex;

            if (index >= 0)
            {
                MessageBox.Show(Project.Search[listBox1.SelectedItem.ToString()]);
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddCache(Project.Search[listBox1.SelectedItem.ToString()], listBox1.SelectedItem.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ID = Project.List[ListBox_Main.SelectedItem.ToString()];

            listBox1.Items.Clear();
            //MessageBox.Show(Project.Extension[ID].Versions.ToString());
            foreach (var item in Project.Extension[ID].Versions)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var item in Project.CodeVersions)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var item in Project.AvailableVersions)
            {
                listBox1.Items.Add(item);
            }
        }
    }
}