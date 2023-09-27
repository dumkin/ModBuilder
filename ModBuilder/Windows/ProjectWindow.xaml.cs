using System;
using System.Windows;
using ModBuilder.Library.Classes;

namespace ModBuilder.Windows;

public partial class ProjectWindow : Window
{
    public ProjectWindow()
    {
        try
        {
            InitializeComponent();

            ProjectList.Load();

            var projects = ProjectList.Get();
            foreach (var project in projects)
            {
                ControlProjects.Items.Add(project);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ControlRemove_Click(object sender, EventArgs e)
    {
        IsEnabled = false;

        try
        {
            var name = ControlProjects.SelectedItem.ToString();

            ProjectList.Remove(name);

            ControlProjects.Items.RemoveAt(ControlProjects.SelectedIndex);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsEnabled = true;
        }
    }

    private async void ControlLoad_Click(object sender, EventArgs e)
    {
        IsEnabled = false;

        try
        {
            MainWindow.Modpack = await ProjectList.GetModpack(ControlProjects.SelectedItem.ToString());

            Hide();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsEnabled = true;
        }
    }

    private async void ControlNew_Click(object sender, EventArgs e)
    {
        IsEnabled = false;

        try
        {
            var name = ControlNewName.Text;

            await ProjectList.Add(name);

            MainWindow.Modpack = await ProjectList.GetModpack(name);

            Hide();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsEnabled = true;
        }
    }

    private void ControlProjects_Changed(object sender, EventArgs e)
    {
        try
        {
            ControlLoad.IsEnabled = ControlRemove.IsEnabled = ControlProjects.SelectedIndex >= 0;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}