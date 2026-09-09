namespace ParetoTool.Models;

/// <summary>
/// Represents a row in the Pareto chart data, containing the rank, category, value, percentage, cumulative value, and cumulative percentage.
/// </summary>
public class ParetoRow
{
    /// <summary>
    /// Gets the rank of the category based on its value, with 1 being the highest value.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    /// Gets the category name associated with this row.
    /// </summary>
    public string Category { get; init; } = string.Empty;
    /// <summary>
    /// Gets the value associated with this category.
    /// </summary>
    public double Value { get; init; }

    /// <summary>
    /// Gets the percentage of the total value that this category represents.
    /// </summary>
    public double Percentage { get; init; }

    /// <summary>
    /// Gets the cumulative value up to and including this category.
    /// </summary>
    public double CumulativeValue { get; init; }

    /// <summary>
    /// Gets the cumulative percentage up to and including this category.
    /// </summary>
    public double CumulativePercentage { get; init; }
}
