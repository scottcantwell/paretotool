using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParetoTool.Models;

namespace ParetoTool.ViewModels
{
    internal partial class ProjectViewModel : ObservableObject
    {
        
        private Project _project;   
        public ProjectViewModel() 
        {

            _project = new Project();

            // initialize generated backing fields so UI shows model values
            _name = _project.Name ?? "New Project";
            _description  = _project.Description ?? string.Empty;

        }

        public string WindowTitle => _windowTitle;
        
        private string _windowTitle = "Pareto Tool - New Project";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ProjectName))]
        private string _name = "New Project";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ProjectDescription))]
        private string _description = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Tags))]
        private List<Tag> tagsList = new List<Tag>();


        public ProjectViewModel(string fileName)
        {

            //_project = ParetoTool.Models.ParetoTool.LoadProject(fileName);

        }

        private void Tags_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Tag newTag in e.NewItems)
                {
                    _project.AddTag(newTag);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Tag oldTag in e.OldItems)
                {
                    _project.RemoveTag(oldTag);
                }
            }
        }   

       public string ProjectName
        {
            get => _project.Name;
            set
            {
                if (_project.Name != value)
                {
                    _project.Name = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }


        public string ProjectDescription
        {
            get => _project.Description;
            set
            {
                if (_project.Description != value)
                {
                    _project.Description = value;
                    OnPropertyChanged(nameof(ProjectDescription));
                }
            }
        }   

        public List<Tag> Tags
        {
            get => _project.Tags.ToList();
            set
            {
                if (_project.Tags != value)
                {
                    _project.Tags = value;
                    OnPropertyChanged(nameof(Tags));
                }
            }
        }


        [RelayCommand]
        public void SaveProject()
        {

            if (_project.IsDirty)
            {

                if (_project.Id != Guid.Empty)
                {

                    SaveExistingProject();

                }

               
                if (_project.Id == Guid.Empty)
                {
                    SaveNewProject();

                }


            }

          
        }
                
        private bool SaveNewProject()
        {
          
            _project.Id = Guid.NewGuid();

            //Save the project to the file.

            _project.Reset();

            Models.ParetoTool.AddProject(_project);

            _windowTitle = $"Pareto Tool - {_project.Name}";

            return true;

        }

        private bool SaveExistingProject()
        {
           
            var currentProject = Models.ParetoTool.CurrentProject;

            //Save the current project to the file.

            //Remove the current project from the ParetoTool

            if (currentProject != null)
            {

                Models.ParetoTool.RemoveProject(currentProject);

            }

            _project.Reset();

            //Add the updated project to the ParetoTool
            Models.ParetoTool.AddProject(_project);

            _windowTitle = $"Pareto Tool - {_project.Name}";    

            return true;

        }

        [RelayCommand]
        public void LoadProject(string fileName)
        {

            Project project = new Project();

            project.Name = "";

            //project.Id = null;

            project.Description = "";

            _project = project;

            _windowTitle = $"Pareto Tool - {_project.Name}";    

        }

    }
}
