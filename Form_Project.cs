using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ModBuilder.Project;

namespace ModBuilder
{
    public partial class Form_Project : Form
    {
        public Form_Project()
        {
            InitializeComponent();

            if (File.Exists(Directory.GetCurrentDirectory() + "\\projects.json"))
            {
                Projects.Data = Config.Load<List<String>>(Directory.GetCurrentDirectory() + "\\projects.json");
                Projects.Repair();
            }

            Projects.Data = Config.Load<List<String>>(Directory.GetCurrentDirectory() + "\\projects.json");

            Projects.Repair();
            Config.Save(Projects.Data, Directory.GetCurrentDirectory() + "\\projects.json");

            foreach (var Item in Projects.Data)
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
            string Path = ListBox_Projects.SelectedItem.ToString();

            Projects.Data.Remove(Path);
            ListBox_Projects.Items.RemoveAt(ListBox_Projects.SelectedIndex);

            Config.Save(Projects.Data, Directory.GetCurrentDirectory() + "\\projects.json");

            File.Delete(Path);
        }

        private void Button_Load_Click(object sender, EventArgs e)
        {
            Vault.File = ListBox_Projects.SelectedItem.ToString();
            Hide();
        }

        private void Button_New_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog.FileName += ".mbp";

                ListBox_Projects.Items.Add(SaveFileDialog.FileName);

                File.Create(SaveFileDialog.FileName);
                Projects.Data.Add(SaveFileDialog.FileName);

                Config.Save(Projects.Data, Directory.GetCurrentDirectory() + "\\projects.json");

                Vault.File = SaveFileDialog.FileName;
                Hide();
            }
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