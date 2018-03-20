using ModBuilder.Extension;
using ModBuilder.Project;
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
                Parse.AsyncGetImageURL(PProject.SExtension_ID[i]);
            }
        }
    }
}