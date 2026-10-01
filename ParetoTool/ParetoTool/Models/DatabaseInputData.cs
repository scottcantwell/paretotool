using ParetoTool.Enums;
using ParetoTool.Interfaces;

namespace ParetoTool.Models
{
    internal class DatabaseInputData : IInputData
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DataSource DataSourceType => throw new NotImplementedException();
    }
}