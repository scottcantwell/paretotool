using CommunityToolkit.Mvvm.Input;
using ParetoTool.Models;
using System.Windows;

namespace ParetoTool.ViewModels
{
    internal partial class ProjectViewModel
    {
        private Project _project;   
        public ProjectViewModel() 
        {

            _project = new Project();
           

        }

        public ProjectViewModel(string fileName)
        {

            //_project = ParetoTool.Models.ParetoTool.LoadProject(fileName);

        }

        [RelayCommand]
        public void SaveProject()
        {
            if (_project.IsDirty)
            {

                var result = MessageBox.Show("The project has unsaved changes. Do you want to save the changes?", "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
             
                if (result == MessageBoxResult.Yes)
                {
                    //Save New Project
                    if (_project.Id == Guid.Empty)
                    {
                        _project.Id = Guid.NewGuid();

                        Models.ParetoTool.AddProject(_project);

                    }

                    //Save Existing Project
                    if (_project.Id != Guid.Empty)
                    {
                     
                        var currentProject = Models.ParetoTool.CurrentProject;

                        //Save the current project

                        //Remove the current project from the ParetoTool

                        if (currentProject != null)
                        {

                            Models.ParetoTool.RemoveProject(currentProject);

                        }

                        //Save the updated project

                        //Add the updated project to the ParetoTool
                        Models.ParetoTool.AddProject(_project);

                    }

                    // Logic to save the project
                ;
                    MessageBox.Show("Project saved successfully!", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else if (result == MessageBoxResult.No)
                {
                    
                    _project.Reset(); // Reset the dirty flag

                }
                else if (result == MessageBoxResult.Cancel)
                {
                   
                    return;
                }   
            }
            // Logic to save the project
            Models.ParetoTool.AddProject(_project);
            MessageBox.Show("Project saved successfully!", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        [RelayCommand]
        public void LoadProject(string fileName)
        {


        }

    }
}
