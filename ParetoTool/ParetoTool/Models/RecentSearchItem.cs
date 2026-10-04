using Newtonsoft.Json;

namespace ParetoTool.Models
{

    /// <summary>
    /// Represents a recent search item with its name.
    /// </summary>
    public class RecentSearchItem
    {


        /// <summary>
        /// Gets or sets the search text of the recent search item.
        /// </summary>
        public string SearchText { get; set; }

    }

    /// <summary>
    /// Represents a recent search item in the recent searches file with its name.
    /// </summary>
    public class RecentSearchItemData
    {

        /// <summary>
        /// Gets or sets the search text of the recent search item.
        /// </summary>
        [JsonProperty("searchtext")]
        public string SearchText { get; set; }
        public RecentSearchItemData() { }

    }

    public class RecentSearchItemDataList
    {
        [JsonProperty("recentsearches")]
        public List<RecentSearchItemData> RecentSearches { get; set; } = new List<RecentSearchItemData>();

        /// <summary>
        /// Adds a recent search item to the list of recent searches.
        /// </summary>
        /// <param name="item"></param>
        public void Add(RecentSearchItemData item)
        {
            RecentSearches.Add(item);
        }

        /// <summary>
        /// Removes a recent search item from the list of recent searches.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(RecentSearchItemData item)
        {

            RecentSearches.Remove(item);

        }

        /// <summary>
        /// Clears all recent search items from the list of recent searches.
        /// </summary>
        public void Clear()
        {

            RecentSearches.Clear();

        }

        /// <summary>
        /// Loads recent search items from a list of RecentSearchItemData.
        /// </summary>
        /// <param name="items"></param>
        public void LoadFromList(List<RecentSearchItemData> items)
        {
            RecentSearches = items;
        }

        /// <summary>
        /// Saves recent search items to a list of RecentSearchItemData.
        /// </summary>
        /// <param name="items"></param>
        public void Save(List<RecentSearchItemData> items)
        {



        }

    }

}
