using ModBuilder.Extension;
using ModBuilder.Project;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ModBuilder.GUI
{
    public partial class Form_Main : Form
    {
        PProject Project;

        public Form_Main()
        {
            InitializeComponent();

            var Form_Project = new Form_Project();
            Form_Project.ShowDialog();

            Project = Config.Load<PProject>(PList.SelectedProjectFile);
            Project.ToStatic();

            // PProject.SExtension_ID.Add("journeymap");

            // Project.ToExemplar();
            // Config.Save(Project, PList.SelectedProjectFile);
            
            for (var i = 0; i < PProject.SExtension_ID.Count; i++)
            {
                Parse.AsyncGetAllData(PProject.SExtension_ID[i], CallbackGettingData);
            }
        }

        public void CallbackGettingData(String ID)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                PProject test = new PProject();
                test.ToExemplar();
                Config.Save(test, PList.SelectedProjectFile);

                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(PProject.SExtension_ImageURL[ID]);
                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);

                ImageList_Main.Images.Add(ID, img);
                // ImageList_Main.Images.Add(key: ID, image: img);
                //listView1.View = View.Details; // Enables Details view so you can see columns


                listView1.Items.Add(new ListViewItem { Name = ID, ImageKey = ID, Text = PProject.SExtension_Name[ID] }); // Using object initializer to add the text
            }));
        }
    }
}