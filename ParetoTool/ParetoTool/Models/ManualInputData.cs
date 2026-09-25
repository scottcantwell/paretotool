using ParetoTool.Interfaces;

namespace ParetoTool.Models
{
    /// <summary>
    /// Represents a collection of manual input items that can be associated with a Project. This class implements the IInputData interface and 
    /// provides properties for storing a unique identifier and a list of InputItem objects.
    /// </summary>
    public class ManualInputData : IInputData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<InputItem> Items { get; set; } = new List<InputItem>();
        public string Typename { get => nameof(ManualInputData); }
    }

}
