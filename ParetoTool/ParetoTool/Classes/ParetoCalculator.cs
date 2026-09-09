using ParetoTool.Models;

namespace ParetoTool.Classes;

public static class ParetoCalculator
{
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