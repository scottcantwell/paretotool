using Newtonsoft.Json;
using ParetoTool.Interfaces;
using System.ComponentModel;

namespace ParetoTool.Models
{

    /// <summary>
    /// Represents a project with properties for Id, Name, Description, and Author. 
    /// This class is used to encapsulate the details of a project within the ParetoTool application.
    /// </summary>
    internal class Project : INotifyPropertyChanged
    {

        private Guid _id = Guid.NewGuid();

        private string _name = string.Empty;

        private string _version = "1.0.0";

        private string _description = string.Empty;

        private string _appVersion = "1.0.0";

        private DateTime _updatedDate = DateTime.Now;

        private DateTime _createdDate = DateTime.Now;


        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the unique identifier for the project. This property is initialized with a new GUID when a Project instance is created.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get => _id; set => _id = value; }

        /// <summary>
        /// Gets or sets the name of the project. This property is initialized to an empty string and can be set to a meaningful name for the project.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// Gets or sets the description of the project. This property is initialized to an empty string and can be set to provide additional 
        /// context or details about the project.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get => _description; set => _description = value; }

        /// <summary>
        /// Gets or sets the author of the project. This property is initialized to an empty string and can be set to indicate who 
        /// created or is responsible for the project.
        /// </summary>
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the project was created.
        /// </summary>
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get => _createdDate; set => _createdDate = value; }

        /// <summary>
        /// Gets or sets the date and time when the project was last updated.
        /// </summary>
        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime UpdatedDate { get => _updatedDate; set => _updatedDate = value; }

        /// <summary>
        /// Gets or sets the version of the application that created the project. This property is initialized to "1.0.0" and can be updated as the project evolves.
        /// </summary>
        [JsonProperty(PropertyName = "appVersion")]
        public string AppVersion { get => _appVersion; set => _appVersion = value; }

        /// <summary>
        /// Gets or sets the version of the project. This property is initialized to "1.0.0" and can be updated to reflect changes or updates to the project over time.
        /// </summary>
        [JsonProperty(PropertyName = "projectVersion")]
        public string ProjectVersion { get => _version; set => _version = value; }

        /// <summary>
        /// Gets or sets the input data associated with the project. This property can hold any object that implements the IInputData interface,
        /// </summary>
        [JsonProperty(PropertyName = "inputData")]
        public IInputData? InputData { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the project has unsaved changes. This property is not serialized to JSON and 
        /// is used internally to track the state of the project.
        /// </summary>
        [JsonIgnore]
        public bool IsDirty { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the Project class, setting the CreatedDate and UpdatedDate properties to the current date and time.
        /// </summary>
        public Project()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the Project class with specified name, description, and author. 
        /// The CreatedDate and UpdatedDate properties are set to the current date and time.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <param name="description">The description of the project.</param>
        /// <param name="author">The author of the project.</param>
        public Project(string name, string description, string author) : this()
        {
            Name = name;
            Description = description;
            Author = author;

        }


        public void Reset()
        {
            IsDirty = false;
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            IsDirty = true;

        }

    }

}
