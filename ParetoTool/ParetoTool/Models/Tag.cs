namespace ParetoTool.Models
{
    /// <summary>
    /// Represents a tag that can be associated with a project. Each tag has a unique identifier and a name.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag. This property is initialized with a new GUID when a Tag instance is created.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();   
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Initializes a new instance of the Tag class with the specified name.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        public Tag(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Returns a string representation of the Tag object, which is the name of the tag.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

    }
}
