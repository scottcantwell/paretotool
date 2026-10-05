

namespace ParetoTool.Models;

/// <summary>
/// Represents a recent item with its name, path, the time it was accessed, and an associated icon.
/// </summary>
public sealed class RecentItem
{
    public RecentItem(string name, string path, string when, object icon, bool isPinned)
    {
        Name = name;
        Path = path;
        LastAccessed = when;
        Icon = icon;
        IsPinned = isPinned;
    }

    /// <summary>
    /// Gets the complete path of the recent item by combining its path and name.
    /// </summary>
    public string CompletePath => System.IO.Path.Combine(Path, Name);

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
    public string LastAccessed { get; }

    /// <summary>
    /// Gets the icon associated with the recent item.
    /// </summary>
    public object Icon { get; }

    public bool IsPinned { get; }

    public override string ToString()
    {   
        return Name;

    }
}
