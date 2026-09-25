namespace ParetoTool.Models
{

    /// <summary>
    /// Represents a static tool for managing the current project within the ParetoTool application.
    /// </summary>
    internal static class ParetoTool
    {

        /// <summary>
        /// Gets the current project being managed by the ParetoTool. This property holds an instance of the Project class.
        /// </summary>
        public static Project CurrentProject { get; private set; } = new Project();

        /// <summary>
        /// Adds the specified project to the ParetoTool and sets it as the current project.
        /// </summary>
        /// <param name="project">The project to be added.</param>
        public static void AddProject(Project project)
        {

            CurrentProject = project;   

        }

        /// <summary>
        /// Removes the specified project from the ParetoTool. If the project to be removed is the current project, it resets the CurrentProject to a new instance of Project.
        /// </summary>
        /// <param name="project">The project to be removed.</param>
        public static void RemoveProject(Project project)
        {
            if (CurrentProject.Id == project.Id)
            {
                CurrentProject = new Project();
            }   
        }

    }

}
