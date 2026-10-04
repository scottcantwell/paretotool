using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParetoTool.Classes;
using System.IO;

namespace ParetoTool.Models;

public sealed class RecentItemData  
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("path")]
    public string Path { get; set; } 

    [JsonProperty("when")]
    public string When { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Path})";
    }

}

public class RecentItems
{

    [JsonProperty("Items")]
    public IEnumerable<RecentItemData> Items { get; private set; } = Enumerable.Empty<RecentItemData>();

    [JsonConstructor]
    public RecentItems(IEnumerable<RecentItemData> items)
    {
        Items = items;
    }

    public void AddItem(RecentItemData item)
    {
        var itemsList = Items.ToList();
        itemsList.Add(item);
        Items = itemsList;
    }   

    public void RemoveItem(RecentItemData item)
    {

        var itemsList = Items.ToList();
        itemsList.Remove(item);
        Items = itemsList;

    }

    public void LoadItems(string path)
    {
       
        JToken token;
       
        try
        {
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException($"Recent items file not found: {path}");
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

    public void SaveItems(string path)
    {
        string json = JSONUtil.SerializeObject(this);
        System.IO.File.WriteAllText(path, json);
    }

}