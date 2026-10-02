using ParetoTool.Enums;
using ParetoTool.Interfaces;

namespace ParetoTool.Models
{
    public class DatabaseInputData : IInputData
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DataSource DataSourceType => DataSource.Database;

        public IConnection Connection { get; set; }

    }


    public interface IConnection
    {
        Guid Id { get; set; }
        
    }

    public class SQLServerConnection : IConnection
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionString => $"Server={Server};Database={DatabaseName};User Id={DatabaseUser};Password={DatabasePassword};Integrated Security={UseIntegratedSecurity};";   
        public string ProviderName { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public bool UseIntegratedSecurity { get; set; } = false;
        public string DatabasePassword { get; set; } = string.Empty;
        public string DatabaseUser { get; set; } = string.Empty;

    }

    public interface CommandText
    {
        Guid Id { get; set; }
    }

    public class SQLCommandText : CommandText
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CommandText { get; set; } = string.Empty;
    }


}