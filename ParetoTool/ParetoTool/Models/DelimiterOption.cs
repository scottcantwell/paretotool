namespace ParetoTool.Models
{
    public sealed class DelimiterOption
    {
        public string DisplayName { get; init; } = "";
        public string Value { get; init; } = "";
        public bool IsCustom { get; init; }
    }
}