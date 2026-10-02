using Newtonsoft.Json;

namespace ParetoTool.Models
{

    /// <summary>
    /// Represents a collection of projects and tags in the ParetoTool application. This class serves as a container for managing multiple ProjectDB and 
    /// TagDB instances, allowing for the organization and retrieval of project-related data. 
    /// It provides properties to access the list of projects and tags, facilitating the management of project information within the application.
    /// </summary>
    [JsonArray]
    internal class ProjectsDB
    {
        /// <summary>
        /// Gets or sets the collection of projects in the database. Each project is represented by an instance of the ProjectDB class, which contains
        /// the details of the project such as Id, Name, Path, and associated tags.
        /// </summary>
        [JsonProperty(PropertyName = "projects")]
        public IEnumerable<ProjectDB> Projects { get; set; } = new List<ProjectDB>();

        /// <summary>
        /// Gets or sets the collection of tags in the database. Each tag is represented by an instance of the TagDB class, which contains
        /// the tag name and the list of project IDs associated with the tag.
        /// </summary>
        [JsonProperty(PropertyName = "bytag")]
        public IEnumerable<TagDB> Tags { get; set; } = new List<TagDB>();


    }

    /// <summary>
    /// Represents a project with properties for Id, Name, Path, and associated tags. This class is used to encapsulate the 
    /// details of a project within the ParetoTool application.
    /// </summary>
    public class ProjectDB
    {
        /// <summary>
        /// Gets or sets the unique identifier for the project. This property is initialized to Guid.Empty.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// Gets or sets the name of the project. This property is initialized to an empty string.
        /// </summary>  
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file system path where the project is stored. This property is initialized to an empty string.
        /// </summary>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of tags associated with the project. Each tag is represented by an instance of the ProjectTagDB class,
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<ProjectTagDB> Tags { get; set; } = new List<ProjectTagDB>();   


    }

    /// <summary>
    /// Represents a collection of tags associated with a project. Each tag is represented by a string and can be linked to 
    /// multiple projects through their unique identifiers (GUIDs). 
    /// This class is used to manage the relationship between tags and projects within the ParetoTool application.
    /// </summary>
    public class TagDB
    {
        /// <summary>
        /// Gets or sets the name of the tag. This property is initialized to an empty string.
        /// </summary>
        public string Tag { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of project IDs associated with the tag. Each project ID is represented by a GUID.
        /// </summary>  
        public IEnumerable<Guid> Projects { get; set;} = new List<Guid>();

    }
    
    /// <summary>
    /// Represents a tag associated with a project. This class is used to encapsulate the details of a tag within the ParetoTool application.
    /// </summary>
    public class ProjectTagDB
    {

        /// <summary>
        /// Gets or sets the name of the tag. This property is initialized to an empty string.
        /// </summary>
        public string Tag { get; set; } = string.Empty;
      
    }
}
