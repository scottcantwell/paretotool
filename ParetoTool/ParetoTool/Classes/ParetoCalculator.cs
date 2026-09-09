using ParetoTool.Models;

namespace ParetoTool.Classes;

/// <summary>
/// Provides functionality to transform a collection of input items into a list of Pareto rows, calculating ranks, percentages, and cumulative values for each category.
/// </summary>
public static class ParetoCalculator
{
    /// <summary>
    /// Transforms a collection of input items into a list of Pareto rows, calculating ranks, percentages, and cumulative values for each category. 
    /// Only valid items with non-empty categories and positive values are considered. The resulting list is sorted by value in
    /// descending order and then by category name.
    /// </summary>
    /// <param name="items">The collection of input items to transform.</param>
    /// <returns>A list of Pareto rows representing the transformed input items.</returns>
    public static List<ParetoRow> Transform(IEnumerable<InputItem> items)
    {
        var valid = items
            .Where(i => !string.IsNullOrWhiteSpace(i.Category) && i.Value > 0)
            .GroupBy(i => i.Category.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => new { Category = g.First().Category.Trim(), Value = g.Sum(x => x.Value) })
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Category)
            .ToList();

        double total = valid.Sum(x => x.Value);
        if (total <= 0)
            return new List<ParetoRow>();

        double running = 0;
        var rows = new List<ParetoRow>(valid.Count);

        for (int i = 0; i < valid.Count; i++)
        {
            running += valid[i].Value;
            rows.Add(new ParetoRow
            {
                Rank = i + 1,
                Category = valid[i].Category,
                Value = valid[i].Value,
                Percentage = valid[i].Value / total * 100.0,
                CumulativeValue = running,
                CumulativePercentage = running / total * 100.0
            });
        }

        return rows;
    }
}