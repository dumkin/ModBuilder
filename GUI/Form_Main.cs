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
                ListView_Main.Items.Add(new ListViewItem { Name = Item.Key, ImageKey = Item.Key, Text = Item.Value.Name });
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

        public void CallbackCheckingCache(String ID)
        {
            Project.CountCheckedCache++;

            if (Project.CountCheckedCache == Project.Extension.Count)
            {
                Config.Save(Project, Projects.SelectedProjectFile);

                /*
                BeginInvoke(new MethodInvoker(delegate
                {
                    Enabled = true;
                }));
                */
            }

            BeginInvoke(new MethodInvoker(delegate
            {
                ImageList_Main.Images.Add(ID, Project.Extension[ID].Image);
            }));
        }

        private void ListView_Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ListView_Main.SelectedIndices.Count > 0)
            {
                if (ListView_Main.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    ContextMenuStrip_Main.Show(Cursor.Position);
                }
            }
        }
    }
}