using System.Text;

namespace ParetoTool.Models
{
    public sealed class EncodingOption
    {
        public string DisplayName { get; init; } = "";
        public Encoding Encoding { get; init; } = Encoding.UTF8;
    }
}