namespace ParetoTool.Models;

/// <summary>
/// Represents a recent item with its name, path, the time it was accessed, and an associated icon.
/// </summary>
public sealed class RecentItem
{
    public RecentItem(string name, string path, string when, object icon)
    {
        Name = name;
        Path = path;
        When = when;
        Icon = icon;
    }

    /// <summary>
    /// Gets the name of the recent item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the path of the recent item.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the time when the recent item was accessed.
    /// </summary>
    public string When { get; }

    /// <summary>
    /// 
    /// </summary>
    public object Icon { get; }

    public override string ToString()
    {
        
        return Name;

    }
}
