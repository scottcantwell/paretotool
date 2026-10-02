namespace ParetoTool.Models.ProjectsDB
{

    public class ProjectListDB
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProjectName { get; set; } = string.Empty;
        
        public string Path { get; set; } = string.Empty;

    }

}