using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ModBuilder.Project;

namespace ModBuilder
{
    public partial class Form_Project : Form
    {
        PList ProjectsList = new PList();

        public Form_Project()
        {
            InitializeComponent();

            if (File.Exists(Directory.GetCurrentDirectory() + "\\projects.json"))
            {
                ProjectsList.Data = Config.Load<List<String>>(Directory.GetCurrentDirectory() + "\\projects.json");
                ProjectsList.Repair();
            }

            Config.Save(ProjectsList.Data, Directory.GetCurrentDirectory() + "\\projects.json");

            foreach (var Item in ProjectsList.Data)
            {
                ListBox_Projects.Items.Add(Item);
            }
        }

        private void Form_Project_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            Enabled = false;

            string Path = ListBox_Projects.SelectedItem.ToString();

            ProjectsList.Data.Remove(Path);
            ListBox_Projects.Items.RemoveAt(ListBox_Projects.SelectedIndex);

            Config.Save(ProjectsList.Data, Directory.GetCurrentDirectory() + "\\projects.json");

            File.Delete(Path);

            Enabled = true;
        }

        private void Button_Load_Click(object sender, EventArgs e)
        {
            Enabled = false;

            PList.SelectedProjectFile = ListBox_Projects.SelectedItem.ToString();
            Hide();

            Enabled = true;
        }

        private void Button_New_Click(object sender, EventArgs e)
        {
            Enabled = false;

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                PList.SelectedProjectFile = SaveFileDialog.FileName + ".mbp";

                ListBox_Projects.Items.Add(PList.SelectedProjectFile);
                ProjectsList.Data.Add(PList.SelectedProjectFile);
                Config.Save(ProjectsList.Data, Directory.GetCurrentDirectory() + "\\projects.json");

                PProject EmptyProject = new PProject();
                Config.Save(EmptyProject, PList.SelectedProjectFile);

                Hide();
            }

            Enabled = true;
        }

        private void ListBox_Projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBox_Projects.SelectedIndex >= 0)
            {
                Button_Load.Enabled = true;
                Button_Delete.Enabled = true;
            }
            else
            {
                Button_Load.Enabled = false;
                Button_Delete.Enabled = false;
            }
        }
    }
}