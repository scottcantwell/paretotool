using Newtonsoft.Json;

namespace ParetoTool.Models;

/// <summary>
/// Represents a recent item in the recent items file with its name, path, and the time it was accessed.
/// </summary>
public sealed class RecentItemData  
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("path")]
    public string Path { get; set; } 

    [JsonProperty("when")]
    public string When { get; set; }

    [JsonProperty("pinned")]
    public string Pinned { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Path})";
    }

}
