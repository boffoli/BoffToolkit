using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoffToolkit.Configuration {
    /// <summary>
    /// Fornisce funzionalità per aggiornare un file JSON.
    /// </summary>
    public class JsonFileUpdater {
        /// <summary>
        /// Aggiorna un valore specifico in un file JSON.
        /// </summary>
        /// <param name="configFilePath">Il percorso del file di configurazione da aggiornare.</param>
        /// <param name="keyPath">Il percorso della chiave da aggiornare nel formato 'Sezione:Chiave'.</param>
        /// <param name="newValue">Il nuovo valore da impostare per la chiave specificata.</param>
        /// <exception cref="FileNotFoundException">Lanciata se il file di configurazione non è trovato nel percorso specificato.</exception>
        /// <exception cref="JsonException">Lanciata se il contenuto del file non è un JSON valido.</exception>
        /// <exception cref="ArgumentException">Lanciata se il percorso della chiave non è nel formato corretto o non corrisponde alla struttura del JSON.</exception>
        public static void UpdateValue(string configFilePath, string keyPath, string newValue) {
            // Verifica l'esistenza del file di configurazione
            if (!File.Exists(configFilePath)) {
                throw new FileNotFoundException("Il file di configurazione non è stato trovato.", configFilePath);
            }

            // Carica il contenuto JSON dal file
            var jsonContent = File.ReadAllText(configFilePath);
            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonContent) ?? throw new JsonException("Il contenuto del file non è un JSON valido.");

            // Imposta il nuovo valore per la chiave specificata
            SetJsonValue(jsonObject, keyPath, newValue);

            // Salva il JSON aggiornato nel file
            var updatedJsonContent = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            File.WriteAllText(configFilePath, updatedJsonContent);
        }

        /// <summary>
        /// Imposta il valore specificato nel JObject.
        /// </summary>
        /// <param name="jsonObject">Il JObject da modificare.</param>
        /// <param name="keyPath">Il percorso della chiave nel formato 'Sezione:Chiave'.</param>
        /// <param name="value">Il nuovo valore da impostare.</param>
        private static void SetJsonValue(JObject jsonObject, string keyPath, string value) {
            // Suddivide il percorso della chiave in sezioni
            var pathParts = keyPath.Split(':');
            if (pathParts.Length < 2) {
                throw new ArgumentException("Il percorso della chiave deve essere nel formato 'Sezione:Chiave' o 'Sezione:Sezione:...:Chiave'");
            }

            // Naviga attraverso le sezioni del JSON
            var currentObject = jsonObject;
            foreach (var part in pathParts.Take(pathParts.Length - 1).SelectMany(section => section.Split('.'))) {
                Object? currentObjectPart = currentObject[part];

                currentObject = currentObjectPart != null ? (JObject)currentObjectPart : throw new InvalidOperationException("currentObject non può essere null.");
                currentObject = currentObject ?? throw new ArgumentException("Il percorso della chiave non corrisponde alla struttura del JSON");
            }


            // Imposta il nuovo valore per la chiave finale
            currentObject[pathParts.Last()] = value;
        }
    }
}