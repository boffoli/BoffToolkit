using System.Reflection;
using BoffToolkit.JsonValidator;
using Microsoft.Extensions.Configuration;

namespace BoffToolkit.Configuration.JsonSettings {
    /// <summary>
    /// Rappresenta una risorsa, includendo sia file di configurazione che schemi JSON.
    /// </summary>
    public struct Resource {
        public string Name;
        public Assembly Assembly;
    }

    /// <summary>
    /// Fornisce funzionalità per costruire e configurare un oggetto di configurazione basato su JSON.
    /// </summary>
    public class JsonSettingsBuilder {
        private readonly Core.ConfigurationManager _configManager = new Core.ConfigurationManager();

        /// <summary>
        /// Carica la configurazione JSON da una risorsa incorporata e la valida opzionalmente contro uno schema JSON incorporato.
        /// Se lo schema non è fornito, non viene eseguita alcuna validazione dello schema.
        /// </summary>
        /// <param name="configResource">La risorsa di configurazione.</param>
        /// <param name="schemaResource">La risorsa di schema opzionale.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, Resource? schemaResource = null) {
            string jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            string schemaContent = schemaResource.HasValue
                ? ReadSchemaFromResource(schemaResource.Value.Name, schemaResource.Value.Assembly)
                : string.Empty;
            ValidateJson(jsonContent, schemaContent);
            _configManager.AddJsonStream(jsonContent);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON da un file e la valida opzionalmente contro uno schema JSON fornito tramite percorso file.
        /// Se il percorso dello schema è "" o null, non viene eseguita alcuna validazione dello schema.
        /// </summary>
        /// <param name="configFilePath">Percorso del file di configurazione JSON.</param>
        /// <param name="schemaFilePath">Percorso del file dello schema JSON. Se "" o null, la validazione dello schema non viene eseguita.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(string configFilePath, string schemaFilePath) {
            string jsonContent = ReadJsonContentFromFile(configFilePath);
            string schemaContent = !string.IsNullOrEmpty(schemaFilePath) ? ReadSchemaFromFile(schemaFilePath) : string.Empty;
            if (!string.IsNullOrEmpty(schemaContent)) {
                ValidateJson(jsonContent, schemaContent);
            }
            _configManager.AddJsonFile(configFilePath);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON da una risorsa incorporata e la valida opzionalmente contro uno schema JSON specificato da un percorso file.
        /// Se il percorso dello schema è "" o null, non viene eseguita alcuna validazione dello schema.
        /// </summary>
        /// <param name="configResource">La risorsa di configurazione.</param>
        /// <param name="schemaFilePath">Percorso del file dello schema JSON. Se "" o null, la validazione dello schema non viene eseguita.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(Resource configResource, string schemaFilePath) {
            string jsonContent = ReadJsonContentFromResource(configResource.Assembly, configResource.Name);
            string schemaContent = !string.IsNullOrEmpty(schemaFilePath) ? ReadSchemaFromFile(schemaFilePath) : string.Empty;
            if (!string.IsNullOrEmpty(schemaContent)) {
                ValidateJson(jsonContent, schemaContent);
            }
            _configManager.AddJsonStream(jsonContent);
            return this;
        }

        /// <summary>
        /// Carica la configurazione JSON da un file e la valida opzionalmente contro uno schema JSON incorporato in un assembly.
        /// </summary>
        /// <param name="configFilePath">Percorso del file di configurazione JSON.</param>
        /// <param name="schemaResource">La risorsa dello schema JSON per la validazione.</param>
        /// <returns>L'istanza corrente per chaining di configurazione.</returns>
        public JsonSettingsBuilder WithConfiguration(string configFilePath, Resource schemaResource) {
            string jsonContent = ReadJsonContentFromFile(configFilePath);
            string schemaContent = ReadSchemaFromResource(schemaResource.Name, schemaResource.Assembly);
            ValidateJson(jsonContent, schemaContent);
            _configManager.AddJsonFile(configFilePath);
            return this;
        }

        /// <summary>
        /// Costruisce la configurazione finale basata sui dati forniti.
        /// </summary>
        /// <returns>Un'istanza di <see cref="IConfiguration"/> configurata.</returns>
        public IConfiguration Build() {
            return _configManager.Build();
        }

        // Legge il contenuto JSON da un file.
        private static string ReadJsonContentFromFile(string filePath) {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentException("Il percorso del file non può essere null o vuoto.", nameof(filePath));
            }
            return File.ReadAllText(filePath);
        }

        // Legge il contenuto JSON da una risorsa incorporata.
        private static string ReadJsonContentFromResource(Assembly assembly, string resourceName) {
            if (assembly == null || string.IsNullOrEmpty(resourceName)) {
                throw new ArgumentException("Assembly e nome della risorsa non possono essere null o vuoti.", nameof(resourceName));
            }
            return EmbeddedResourceHandler.ReadResourceAsString(assembly, resourceName);
        }

        // Legge il contenuto dello schema JSON da un file.
        private static string ReadSchemaFromFile(string schemaFilePath) {
            return string.IsNullOrEmpty(schemaFilePath) ? string.Empty : File.ReadAllText(schemaFilePath);
        }

        // Legge il contenuto dello schema JSON da una risorsa incorporata.
        private static string ReadSchemaFromResource(string resourceSchemaName, Assembly? schemaAssembly) {
            if (schemaAssembly == null || string.IsNullOrEmpty(resourceSchemaName)) {
                return string.Empty;
            }
            return EmbeddedResourceHandler.ReadResourceAsString(schemaAssembly, resourceSchemaName);
        }

        // Valida il contenuto JSON contro lo schema JSON specificato.
        private static void ValidateJson(string jsonContent, string schemaContent = "") {
            if (!string.IsNullOrEmpty(schemaContent)) {
                SchemaValidator.Validate(jsonContent, schemaContent);
            }
        }
    }
}
