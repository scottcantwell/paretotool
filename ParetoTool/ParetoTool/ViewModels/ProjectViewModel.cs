using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ParetoTool.Models;
using System.Text.RegularExpressions;

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
        
        private string _windowTitle = "ParetoTool - New Project";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        private bool _save = false;

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
        public bool CanSave
        {
            get => _save;
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

        [RelayCommand]
        private void AddTag()
        {
            //var name = TagInput.Trim();
            //if (string.IsNullOrWhiteSpace(name))
            //    return;

            //if (Tags.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            //{
            //    TagInput = string.Empty;
            //    return;
            //}

            //Tags.Add(new Tag { Name = name });
            //TagInput = string.Empty;
        }

        [RelayCommand]
        private void RemoveTag(Tag tag)
        {
            Tags.Remove(tag);
        }

        private bool SaveNewProject()
        {
          
            _project.Id = Guid.NewGuid();

            var files = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "*.pareto");

            List<int> fileCounts = new List<int>();

            string newProjectFilename = "NewProject";

            Regex filecountRegex = new Regex(@"NewProject(\d+)\.pareto");

            foreach ( var file in files )
            {

                if (filecountRegex.IsMatch(System.IO.Path.GetFileName(file)))
                {
                    var match = filecountRegex.Match(System.IO.Path.GetFileName(file));
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int count))
                    {
                        fileCounts.Add(count);
                    }
                }

            }

            int fileCount = fileCounts.Count > 0 ? fileCounts.Max() + 1 : 1;

            newProjectFilename = $"NewProject{fileCount}";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "ParetoTool files (*.pareto)|*.pareto|All files (*.*)|*.*",
                Title = "Save Project As...",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),                 

                FileName = $"{newProjectFilename}.pareto"

            };  

            if (saveFileDialog.ShowDialog() == true)
            {
                _project.Save(saveFileDialog.FileName);
            }
            else
            {
                return false;
            }   

            _project.Reset();

            Models.ParetoTool.AddProject(_project);

            _windowTitle = $"Pareto Tool - {_project.Name}";

            _save = false;

            return true;

        }

        private bool SaveExistingProject()
        {
           
            var currentProject = Models.ParetoTool.CurrentProject;

            //Save the current project to the file.

            _project.Save("");

            //Remove the current project from the ParetoTool

            if (currentProject != null)
            {

                Models.ParetoTool.RemoveProject(currentProject);

            }

            _project.Reset();

            //Add the updated project to the ParetoTool
            Models.ParetoTool.AddProject(_project);

            _windowTitle = $"Pareto Tool - {_project.Name}";
            _save = false;
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
