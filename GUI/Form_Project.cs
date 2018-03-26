using System;
using System.IO;
using System.Windows.Forms;
using ModBuilder.ProjectSystem;
using ModBuilder.Utilities;

namespace ModBuilder
{
    public partial class Form_Project : Form
    {
        Projects Projects = new Projects();

        public Form_Project()
        {
            InitializeComponent();

            if (File.Exists(Directory.GetCurrentDirectory() + "\\projects.json"))
            {
                Projects = Config.Load<Projects>(Directory.GetCurrentDirectory() + "\\projects.json");

                if (!Projects.Valid())
                {
                    Projects.Repair();
                    Config.Save(Projects, Directory.GetCurrentDirectory() + "\\projects.json");
                }
            }
            else
            {
                Config.Save(Projects, Directory.GetCurrentDirectory() + "\\projects.json");
            }

            foreach (var Item in Projects.Files)
            {
                ListBox_Projects.Items.Add(Item);
            }
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            Enabled = false;

            String Path = ListBox_Projects.SelectedItem.ToString();

            Projects.Files.Remove(Path);
            Config.Save(Projects.Files, Directory.GetCurrentDirectory() + "\\projects.json");

            File.Delete(Path);

            ListBox_Projects.Items.RemoveAt(ListBox_Projects.SelectedIndex);

            Enabled = true;
        }
        private void Button_Load_Click(object sender, EventArgs e)
        {
            Enabled = false;

            Projects.SelectedProjectFile = ListBox_Projects.SelectedItem.ToString();
            Hide();

            Enabled = true;
        }
        private void Button_New_Click(object sender, EventArgs e)
        {
            Enabled = false;

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Projects.SelectedProjectFile = SaveFileDialog.FileName + ".mbp";

                Projects.Files.Add(Projects.SelectedProjectFile);
                Config.Save(Projects, Directory.GetCurrentDirectory() + "\\projects.json");

                Project EmptyProject = new Project();
                Config.Save(EmptyProject, Projects.SelectedProjectFile);

                ListBox_Projects.Items.Add(Projects.SelectedProjectFile);

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