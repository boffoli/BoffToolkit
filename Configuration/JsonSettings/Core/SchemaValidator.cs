using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;

namespace BoffToolkit.Configuration.JsonSettings.Core
{
    /// <summary>
    /// Esegue la validazione delle configurazioni JSON rispetto a uno schema JSON fornito.
    /// </summary>
    internal class SchemaValidator
    {
        private string? _jsonSchema;
        private List<string> _jsonContents = new List<string>();

        /// <summary>
        /// Imposta lo schema JSON utilizzato per la validazione.
        /// </summary>
        /// <param name="jsonSchema">Lo schema JSON da utilizzare per la validazione.</param>
        public void SetSchema(string jsonSchema)
        {
            _jsonSchema = jsonSchema;
        }

        /// <summary>
        /// Aggiunge il contenuto JSON da un file specificato.
        /// </summary>
        /// <param name="configFilePath">Il percorso del file JSON da cui leggere il contenuto.</param>
        public void AddJsonContentByFile(string configFilePath)
        {
            string content = ReadFileToString(configFilePath);
            AddJsonContent(content);
        }

        /// <summary>
        /// Aggiunge un contenuto JSON da validare.
        /// </summary>
        /// <param name="jsonContent">Il contenuto JSON da aggiungere alla lista di validazione.</param>
        public void AddJsonContent(string jsonContent)
        {
            _jsonContents.Add(jsonContent);
        }

        /// <summary>
        /// Esegue la validazione di tutti i contenuti JSON rispetto allo schema fornito.
        /// </summary>
        /// <exception cref="JsonException">Viene generata se uno qualsiasi dei contenuti JSON non è valido rispetto allo schema.</exception>
        public void ValidateJsons()
        {
            if (string.IsNullOrWhiteSpace(_jsonSchema))
            {
                return;
            }

            foreach (var jsonContent in _jsonContents)
            {
                ValidateJsonAgainstSchema(jsonContent, _jsonSchema);
            }
        }

        // Metodo privato per leggere un file e restituire il suo contenuto come stringa
        private string ReadFileToString(string filePath)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                throw new IOException($"Errore durante l'apertura o la lettura del file: {ex.Message}");
            }
        }

        // Metodo interno per la validazione JSON
        private void ValidateJsonAgainstSchema(string json, string schemaJson)
        {
            JSchema schema = JSchema.Parse(schemaJson);
            JObject jsonObject = JObject.Parse(json);

            if (!jsonObject.IsValid(schema, out IList<string> errorMessages))
            {
                string errorMessage = $"La configurazione non è valida rispetto allo schema JSON fornito. Errori: {string.Join(", ", errorMessages)}";
                throw new JsonException(errorMessage);
            }
        }

        /// <summary>
        /// Metodo statico per la validazione di un JSON rispetto a uno schema. Genera un'eccezione se il contenuto non è valido.
        /// </summary>
        /// <param name="jsonContent">Il contenuto JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui validare il contenuto.</param>
        /// <exception cref="JsonException">Se il contenuto JSON non è valido rispetto allo schema fornito.</exception>
        public static void Validate(string jsonContent, string jsonSchema)
        {
            JSchema schema = JSchema.Parse(jsonSchema);
            JObject jsonObject = JObject.Parse(jsonContent);

            if (!jsonObject.IsValid(schema, out IList<string> errorMessages))
            {
                string errorMessage = $"Il contenuto JSON non è valido rispetto allo schema fornito. Errori: {string.Join(", ", errorMessages)}";
                throw new JsonException(errorMessage);
            }
        }
    }
}
