using ParetoTool.Interfaces;

namespace ParetoTool.Models
{
    /// <summary>
    /// Represents a file-based input data source that can be associated with a Project. This class implements the IInputData interface and
    /// </summary>
    public class FileInputData : IInputData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FilePath { get; set; } = string.Empty;
        public string Typename { get => nameof(FileInputData); }    

    }

}
