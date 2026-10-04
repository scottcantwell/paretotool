namespace ParetoTool.Models;

public sealed class RecentItem
{
    public RecentItem(string name, string path, string when, object icon)
    {
        Name = name;
        Path = path;
        When = when;
        Icon = icon;
    }

    public string Name { get; }
    public string Path { get; }
    public string When { get; }
    public object Icon { get; }
}
