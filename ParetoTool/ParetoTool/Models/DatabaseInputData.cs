using ParetoTool.Enums;
using ParetoTool.Interfaces;

namespace ParetoTool.Models
{
    internal class DatabaseInputData : IInputData
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DataSource DataSourceType => DataSource.Database;
    }

    

}