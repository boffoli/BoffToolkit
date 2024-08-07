using System.Reflection;
using BoffToolkit.JsonValidator;
using Microsoft.Extensions.Configuration;

namespace BoffToolkit.Configuration.JsonSettings {
    /// <summary>
    /// Rappresenta una risorsa, includendo sia file di configurazione che schemi JSON.
    /// </summary>
    public struct Resource {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
    }

    /// <summary>
    /// Fornisce funzionalità per costruire e configurare un oggetto di configurazione basato su JSON.
    /// </summary>
    public class JsonSettingsBuilder {
        private readonly Core.ConfigurationManager _configManager = new();

        /// <summary>
        /// Carica la configurazione JSON direttamente dal contenuto fornito e la valida opzionalmente contro uno schema JSON fornito.
        /// </summary>
        /// <param name="jsonContent">Il contenuto del file di configurazione JSON.</param>
        /// <param name="schemaContent">Il contenuto dello schema JSON per la validazione. Se null o vuoto, la validazione dello schema non viene eseguita.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(string jsonContent, string schemaContent = "") {
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON da una risorsa incorporata e la valida opzionalmente contro uno schema JSON incorporato.
        /// Se lo schema non è fornito, non viene eseguita alcuna validazione dello schema.
        /// </summary>
        /// <param name="configResource">La risorsa di configurazione.</param>
        /// <param name="schemaResource">La risorsa di schema opzionale.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, Resource? schemaResource = null) {
            var jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            var schemaContent = schemaResource.HasValue
                ? ReadSchemaFromResource(schemaResource.Value.Name, schemaResource.Value.Assembly)
                : string.Empty;
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON direttamente dal contenuto fornito e la valida opzionalmente contro uno schema JSON fornito.
        /// </summary>
        /// <param name="jsonContent">Il contenuto del file di configurazione JSON.</param>
        /// <param name="schemaResource">La risorsa di schema opzionale.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(string jsonContent, Resource schemaResource) {
            var schemaContent = ReadSchemaFromResource(schemaResource.Name, schemaResource.Assembly);
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON da una risorsa incorporata e la valida opzionalmente contro uno schema JSON fornito.
        /// </summary>
        /// <param name="configResource">La risorsa di configurazione.</param>
        /// <param name="schemaContent">Il contenuto dello schema JSON per la validazione. Se null o vuoto, la validazione dello schema non viene eseguita.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, string schemaContent) {
            var jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            AddConfiguration(jsonContent, schemaContent);
            return this;
        }

        /// <summary>
        /// Costruisce la configurazione finale basata sui dati forniti.
        /// </summary>
        /// <returns>Un'istanza di <see cref="IConfiguration"/> configurata.</returns>
        public IConfiguration Build() {
            return _configManager.Build();
        }

        // Legge il contenuto JSON da una risorsa incorporata.
        private static string ReadJsonContentFromResource(Assembly assembly, string resourceName) {
            if (assembly == null || string.IsNullOrEmpty(resourceName)) {
                throw new ArgumentException("Assembly e nome della risorsa non possono essere null o vuoti.", nameof(resourceName));
            }
            return EmbeddedResourceHandler.ReadResourceAsString(assembly, resourceName);
        }

        // Legge il contenuto dello schema JSON da una risorsa incorporata.
        private static string ReadSchemaFromResource(string resourceSchemaName, Assembly? schemaAssembly) {
            if (schemaAssembly == null || string.IsNullOrEmpty(resourceSchemaName)) {
                return string.Empty;
            }
            return EmbeddedResourceHandler.ReadResourceAsString(schemaAssembly, resourceSchemaName);
        }

        // Aggiunge la configurazione JSON e la valida opzionalmente contro uno schema JSON.
        private void AddConfiguration(string jsonContent, string? schemaContent = "") {
            if (!string.IsNullOrEmpty(schemaContent)) {
                ValidateJson(jsonContent, schemaContent);
            }
            _configManager.AddJsonStream(jsonContent);
        }

        // Valida il contenuto JSON contro lo schema JSON specificato.
        private static void ValidateJson(string jsonContent, string schemaContent = "") {
            if (!string.IsNullOrEmpty(schemaContent)) {
                SchemaValidator.Validate(jsonContent, schemaContent);
            }
        }
    }
}