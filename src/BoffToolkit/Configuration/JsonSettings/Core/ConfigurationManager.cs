using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BoffToolkit.Configuration.JsonSettings.Core {
    /// <summary>
    /// Manages configuration settings based on JSON files.
    /// </summary>
    internal class ConfigurationManager {
        private readonly ConfigurationBuilder _builder = new();
        private IConfiguration? _cachedConfiguration;

        /// <summary>
        /// Adds configuration from a JSON stream.
        /// </summary>
        /// <param name="jsonContent">The JSON content to add to the configuration.</param>
        public void AddJsonStream(string jsonContent) {
            _builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)));
        }

        /// <summary>
        /// Adds configuration from a JSON file.
        /// </summary>
        /// <param name="configFilePath">The path to the JSON configuration file.</param>
        public void AddJsonFile(string configFilePath) {
            _builder.AddJsonFile(configFilePath, optional: false, reloadOnChange: true);
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <returns>The constructed configuration object.</returns>
        public IConfiguration Build() {
            _cachedConfiguration ??= _builder.Build();
            return _cachedConfiguration;
        }

        /// <summary>
        /// Prints the configuration as a JSON string to the console.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the configuration has not been built yet.
        /// </exception>
        public void PrintConfigAsJson() {
            if (_cachedConfiguration == null) {
                throw new InvalidOperationException("The configuration has not been built yet.");
            }

            var configData = new Dictionary<string, string>();
            foreach (var section in _cachedConfiguration.AsEnumerable()) {
                if (section.Value != null) {
                    configData.Add(section.Key, section.Value);
                }
            }

            var json = JsonConvert.SerializeObject(configData, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}