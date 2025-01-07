using System.Reflection;
using BoffToolkit.JsonValidator;
using Microsoft.Extensions.Configuration;

namespace BoffToolkit.Configuration.JsonSettings {
    /// <summary>
    /// Represents a resource, including configuration files and JSON schemas.
    /// </summary>
    public struct Resource {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the assembly containing the resource.
        /// </summary>
        public Assembly Assembly { get; set; }
    }

    /// <summary>
    /// Provides functionality to build and configure a JSON-based configuration object.
    /// </summary>
    public class JsonSettingsBuilder {
        private readonly Core.ConfigurationManager _configManager = new();

        /// <summary>
        /// Loads JSON configuration directly from the provided content and optionally validates it against a given JSON schema.
        /// </summary>
        /// <param name="jsonContent">The JSON configuration content.</param>
        /// <param name="schemaContent">The JSON schema content for validation. If null or empty, schema validation is skipped.</param>
        /// <returns>The current instance for chaining configuration calls.</returns>
        public JsonSettingsBuilder WithConfiguration(string jsonContent, string schemaContent = "") {
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Loads JSON configuration from an embedded resource and optionally validates it against an embedded JSON schema.
        /// If the schema is not provided, no schema validation is performed.
        /// </summary>
        /// <param name="configResource">The configuration resource.</param>
        /// <param name="schemaResource">The optional schema resource.</param>
        /// <returns>The current instance for chaining configuration calls.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, Resource? schemaResource = null) {
            var jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            var schemaContent = schemaResource.HasValue
                ? ReadSchemaFromResource(schemaResource.Value.Name, schemaResource.Value.Assembly)
                : string.Empty;
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Loads JSON configuration directly from the provided content and optionally validates it against a given schema resource.
        /// </summary>
        /// <param name="jsonContent">The JSON configuration content.</param>
        /// <param name="schemaResource">The schema resource for validation.</param>
        /// <returns>The current instance for chaining configuration calls.</returns>
        public JsonSettingsBuilder WithConfiguration(string jsonContent, Resource schemaResource) {
            var schemaContent = ReadSchemaFromResource(schemaResource.Name, schemaResource.Assembly);
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Loads JSON configuration from an embedded resource and optionally validates it against a provided JSON schema.
        /// </summary>
        /// <param name="configResource">The configuration resource.</param>
        /// <param name="schemaContent">The JSON schema content for validation. If null or empty, schema validation is skipped.</param>
        /// <returns>The current instance for chaining configuration calls.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, string schemaContent) {
            var jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Builds the final configuration based on the provided data.
        /// </summary>
        /// <returns>An instance of <see cref="IConfiguration"/> containing the built configuration.</returns>
        public IConfiguration Build() {
            return _configManager.Build();
        }

        /// <summary>
        /// Reads the JSON content from an embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>The JSON content as a string.</returns>
        /// <exception cref="ArgumentException">Thrown if the assembly or resource name is null or empty.</exception>
        private static string ReadJsonContentFromResource(Assembly assembly, string resourceName) {
            if (assembly == null || string.IsNullOrEmpty(resourceName)) {
                throw new ArgumentException("Assembly and resource name cannot be null or empty.", nameof(resourceName));
            }
            return EmbeddedResourceHandler.ReadResourceAsString(assembly, resourceName);
        }

        /// <summary>
        /// Reads the JSON schema content from an embedded resource.
        /// </summary>
        /// <param name="resourceSchemaName">The name of the schema resource.</param>
        /// <param name="schemaAssembly">The assembly containing the schema resource.</param>
        /// <returns>The JSON schema content as a string, or an empty string if the schema assembly or resource name is null.</returns>
        private static string ReadSchemaFromResource(string resourceSchemaName, Assembly? schemaAssembly) {
            if (schemaAssembly == null || string.IsNullOrEmpty(resourceSchemaName)) {
                return string.Empty;
            }
            return EmbeddedResourceHandler.ReadResourceAsString(schemaAssembly, resourceSchemaName);
        }

        /// <summary>
        /// Adds JSON configuration and optionally validates it against a JSON schema.
        /// </summary>
        /// <param name="jsonContent">The JSON configuration content.</param>
        /// <param name="schemaContent">The JSON schema content for validation.</param>
        private void AddConfiguration(string jsonContent, string? schemaContent = "") {
            if (!string.IsNullOrEmpty(schemaContent)) {
                ValidateJson(jsonContent, schemaContent!);
            }
            _configManager.AddJsonStream(jsonContent);
        }

        /// <summary>
        /// Validates JSON content against a specified JSON schema.
        /// </summary>
        /// <param name="jsonContent">The JSON content to validate.</param>
        /// <param name="schemaContent">The JSON schema content to validate against.</param>
        private static void ValidateJson(string jsonContent, string schemaContent = "") {
            if (!string.IsNullOrEmpty(schemaContent)) {
                SchemaValidator.Validate(jsonContent, schemaContent);
            }
        }
    }
}