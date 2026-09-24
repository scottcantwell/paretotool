

using Newtonsoft.Json;

namespace ParetoTool.Models
{

    /// <summary>
    /// Represents a project with properties for Id, Name, Description, and Author. 
    /// This class is used to encapsulate the details of a project within the ParetoTool application.
    /// </summary>
    internal class Project
    {

        /// <summary>
        /// Gets or sets the unique identifier for the project. This property is initialized with a new GUID when a Project instance is created.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the project. This property is initialized to an empty string and can be set to a meaningful name for the project.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the project. This property is initialized to an empty string and can be set to provide additional 
        /// context or details about the project.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

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
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the project was last updated.
        /// </summary>
        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the version of the application that created the project. This property is initialized to "1.0.0" and can be updated as the project evolves.
        /// </summary>
        [JsonProperty(PropertyName = "appVersion")]
        public string AppVersion { get; set; } = "1.0.0";


        /// <summary>
        /// Gets or sets the version of the project. This property is initialized to "1.0.0" and can be updated to reflect changes or updates to the project over time.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; } = "1.0.0";  

        /// <summary>
        /// Gets or sets the input data associated with the project. This property can hold any object that implements the IInputData interface,
        /// </summary>
        [JsonProperty(PropertyName = "inputData")]
        public IInputData? InputData { get; set; } = null;   

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

    }

}
