using Newtonsoft.Json;

namespace ParetoTool.Models;

/// <summary>
/// Represents a recent item in the recent items file with its name, path, and the time it was accessed.
/// </summary>
public sealed class RecentItemData  
{
    
    /// <summary>
    /// Gets or sets the name of the recent item.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path of the recent item.
    /// </summary>
    [JsonProperty("path")]
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time when the recent item was accessed.
    /// </summary>
    [JsonProperty("lastaccessed")]
    public string LastAccessed { get; set; } = string.Empty;


    /// <summary>
    /// Get or sets the pinned status of the recent item.
    /// </summary>
    [JsonProperty("pinned")]
    public bool Pinned { get; set; } = false;

    /// <summary>
    /// Returns a string representation of the recent item.
    /// </summary>
    /// <returns>A string containing the name and path of the recent item.</returns>
    public override string ToString()
    {
        return $"{Name} ({Path})";
    }

}
