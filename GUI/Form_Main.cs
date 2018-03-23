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
    }
}