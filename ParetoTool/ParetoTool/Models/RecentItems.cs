using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParetoTool.Classes;
using System.IO;

namespace ParetoTool.Models;

/// <summary>
/// Represents a collection of recent items, providing methods to add, remove, load, and save recent items to a JSON file.  
/// </summary>
public class RecentItems
{

    /// <summary>
    /// Gets the collection of recent items.
    /// </summary>

    [JsonProperty("Items")]
    public IEnumerable<RecentItemData> Items { get; private set; } = Enumerable.Empty<RecentItemData>();

    [JsonConstructor]
    public RecentItems(IEnumerable<RecentItemData> items)
    {
        Items = items;
    }

    /// <summary>
    /// Adds a new recent item to the collection.
    /// </summary>
    /// <param name="item">The recent item to add.</param>
    public void AddItem(RecentItemData item)
    {
        var itemsList = Items.ToList();
        itemsList.Add(item);
        Items = itemsList;
    }

    /// <summary>
    /// Removes a recent item from the collection.
    /// </summary>
    /// <param name="item">The recent item to remove.</param>
    public void RemoveItem(RecentItemData item)
    {

        var itemsList = Items.ToList();
        itemsList.Remove(item);
        Items = itemsList;

    }

    /// <summary>
    /// Loads recent items from a JSON file at the specified path.
    /// </summary>
    /// <param name="path">The path to the JSON file containing recent items.</param>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="InvalidDataException">Thrown if the file contains invalid JSON.</exception>
    public void LoadItems(string path)
    {
       
        JToken token;
       
        try
        {

            if (!System.IO.File.Exists(path))
            {
                //Add logging here.
                return;
            }   
            string json = System.IO.File.ReadAllText(path);

            token = JToken.Parse(json);

            var list = JSONUtil.DeserializeList<RecentItemData>(json);
            Items = list ?? Enumerable.Empty<RecentItemData>();

        }
        catch (JsonReaderException ex)
        {
            throw new InvalidDataException($"Recent items file is not valid JSON: {path}", ex);
        }

    }

    /// <summary>
    /// Saves the current collection of recent items to a JSON file at the specified path.
    /// </summary>
    /// <param name="path">The path to the JSON file where recent items will be saved.</param>
    public void SaveItems(string path)
    {
        string json = JSONUtil.SerializeObject(this);
        System.IO.File.WriteAllText(path, json);
    }

}