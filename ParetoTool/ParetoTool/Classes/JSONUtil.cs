namespace ParetoTool.Classes
{

    /// <summary>
    /// Provides utility methods for serializing and deserializing objects to and from JSON format using the Newtonsoft.Json library.
    /// </summary>
    internal class JSONUtil
    {

        /// <summary>
        /// Serializes the specified object to a JSON string with indented formatting for better readability.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeObject<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Deserializes the specified JSON string to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An object of the specified type deserialized from the JSON string.</returns>
        public static T DeserializeObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }   

    }
}
