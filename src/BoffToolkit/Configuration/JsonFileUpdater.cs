using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoffToolkit.Configuration {
    /// <summary>
    /// Provides functionality for updating a JSON file.
    /// </summary>
    public static class JsonFileUpdater {
        /// <summary>
        /// Updates a specific value in JSON content.
        /// </summary>
        /// <param name="jsonContent">The JSON content to update.</param>
        /// <param name="keyPath">The key path to update, formatted as 'Section:Key'.</param>
        /// <param name="newValue">The new value to set for the specified key.</param>
        /// <returns>The updated JSON content as a string.</returns>
        /// <exception cref="JsonException">Thrown if the JSON content is invalid.</exception>
        /// <exception cref="ArgumentException">Thrown if the key path is incorrectly formatted or does not match the JSON structure.</exception>
        public static string UpdateValue(string jsonContent, string keyPath, string newValue) {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent)
                        ?? throw new JsonException("The content of the file is not valid JSON.");

            // Sets the new value for the specified key
            SetJsonValue(jsonObject, keyPath, newValue);

            // Serializes the updated JSON content
            var updatedJsonContent = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            return updatedJsonContent;
        }

        /// <summary>
        /// Sets the specified value in the <see cref="JObject"/>.
        /// </summary>
        /// <param name="jsonObject">The <see cref="JObject"/> to modify.</param>
        /// <param name="keyPath">The key path, formatted as 'Section:Key'.</param>
        /// <param name="value">The new value to set.</param>
        /// <exception cref="ArgumentException">Thrown if the key path is incorrectly formatted or does not match the JSON structure.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the current JSON object is null.</exception>
        private static void SetJsonValue(JObject jsonObject, string keyPath, string value) {
            // Splits the key path into sections
            var pathParts = keyPath.Split(':');
            if (pathParts.Length < 2) {
                throw new ArgumentException("The key path must be formatted as 'Section:Key' or 'Section:Section:...:Key'");
            }

            // Navigates through the JSON sections
            var currentObject = jsonObject;
            foreach (var part in pathParts.Take(pathParts.Length - 1).SelectMany(section => section.Split('.'))) {
                Object? currentObjectPart = currentObject[part];

                currentObject = currentObjectPart != null ? (JObject)currentObjectPart : throw new InvalidOperationException("currentObject cannot be null.");
                currentObject = currentObject ?? throw new ArgumentException("The key path does not match the JSON structure.");
            }

            // Sets the new value for the final key
            currentObject[pathParts.Last()] = value;
        }
    }
}