namespace ParetoTool.Models;

public class ParetoRow
{
    public int Rank { get; init; }
    public string Category { get; init; } = string.Empty;
    public double Value { get; init; }
    public double Percentage { get; init; }
    public double CumulativeValue { get; init; }
    public double CumulativePercentage { get; init; }
}
