using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using AutoFixture;
using AutoFixture.Kernel;

namespace BoffToolkit.JsonValidator
{
    /// <summary>
    /// Fornisce funzionalità per la validazione di contenuti JSON rispetto a uno schema JSON.
    /// Supporta sia la validazione di stringhe JSON che la validazione di istanze di oggetti,
    /// utilizzando uno schema JSON predefinito o specificato al momento della validazione.
    /// </summary>
    internal class SchemaValidator
    {
        private string? _jsonSchema;
        private static readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// Imposta lo schema JSON da utilizzare per le future validazioni.
        /// </summary>
        /// <param name="jsonSchema">La stringa che rappresenta lo schema JSON.</param>
        public void SetSchema(string jsonSchema)
        {
            _jsonSchema = jsonSchema;
        }

        /// <summary>
        /// Valida una stringa JSON rispetto a uno schema JSON specificato o a uno predefinito.
        /// </summary>
        /// <param name="jsonContent">Il contenuto JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione. Se null, verrà utilizzato lo schema predefinito.</param>
        /// <exception cref="InvalidOperationException">Se lo schema JSON non è stato impostato né fornito.</exception>
        /// <exception cref="JsonException">Se il contenuto JSON non è valido rispetto allo schema fornito.</exception>
        public void Validate(string jsonContent, string? jsonSchema = null)
        {
            string schemaToUse = jsonSchema ?? _jsonSchema ?? throw new InvalidOperationException("Lo schema JSON non è stato impostato.");
            PerformValidation(jsonContent, schemaToUse);
        }

        /// <summary>
        /// Valida un'istanza di oggetto convertendola in una stringa JSON e validandola rispetto a uno schema JSON specificato o predefinito.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione. Se null, verrà utilizzato lo schema predefinito.</param>
        /// <exception cref="InvalidOperationException">Se lo schema JSON non è stato impostato né fornito.</exception>
        /// <exception cref="JsonException">Se l'istanza dell'oggetto convertita in JSON non è valida rispetto allo schema fornito.</exception>
        public void Validate(object instance, string? jsonSchema = null)
        {
            Validate(instance.GetType(), jsonSchema);
        }

        /// <summary>
        /// Genera dinamicamente un'istanza di un dato tipo, la converte in una stringa JSON,
        /// e la valida rispetto a uno schema JSON specificato o predefinito.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto di cui generare un'istanza per la validazione.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione. Se null, verrà utilizzato lo schema predefinito.</param>
        /// <exception cref="InvalidOperationException">Se lo schema JSON non è stato impostato né fornito.</exception>
        /// <exception cref="JsonException">Se l'istanza generata e convertita in JSON non è valida rispetto allo schema fornito.</exception>
        public void Validate(Type type, string? jsonSchema = null)
        {
            string schemaToUse = jsonSchema ?? _jsonSchema ?? throw new InvalidOperationException("Lo schema JSON non è stato impostato.");
            var context = new SpecimenContext(_fixture as ISpecimenBuilder);
            object instance = context.Resolve(type);
            if (instance == null)
            {
                throw new InvalidOperationException($"Impossibile creare un'istanza del tipo {type.FullName}.");
            }
            string instanceJson = JsonConvert.SerializeObject(instance);
            PerformValidation(instanceJson, schemaToUse);
        }

        /// <summary>
        /// Metodo statico per la validazione diretta di una stringa JSON rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="jsonContent">Il contenuto JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione.</param>
        /// <exception cref="JsonException">Se il contenuto JSON non è valido rispetto allo schema fornito.</exception>
        public static void ValidateStatic(string jsonContent, string jsonSchema)
        {
            PerformValidation(jsonContent, jsonSchema);
        }

        /// <summary>
        /// Metodo statico per la validazione di un'istanza di oggetto rispetto a uno schema JSON fornito,
        /// convertendo prima l'oggetto in una stringa JSON.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione.</param>
        /// <exception cref="JsonException">Se l'istanza dell'oggetto convertita in JSON non è valida rispetto allo schema fornito.</exception>
        public static void ValidateStatic(object instance, string jsonSchema)
        {
            string instanceJson = JsonConvert.SerializeObject(instance);
            PerformValidation(instanceJson, jsonSchema);
        }

        /// <summary>
        /// Metodo statico per la generazione dinamica di un'istanza di un dato tipo, la sua conversione in una stringa JSON,
        /// e la validazione rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto di cui generare un'istanza per la validazione.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui eseguire la validazione.</param>
        /// <exception cref="JsonException">Se l'istanza generata e convertita in JSON non è valida rispetto allo schema fornito.</exception>
        public static void ValidateStatic(Type type, string jsonSchema)
        {
            object instance = CreateInstance(type);
            string instanceJson = JsonConvert.SerializeObject(instance);
            PerformValidation(instanceJson, jsonSchema);
        }

        /// <summary>
        /// Esegue la validazione di una stringa JSON rispetto a uno schema JSON.
        /// </summary>
        /// <param name="json">La stringa JSON da validare.</param>
        /// <param name="schemaJson">Lo schema JSON contro cui validare la stringa.</param>
        /// <exception cref="JsonException">Se la stringa JSON non è valida rispetto allo schema fornito.</exception>
        // Assicurati di avere l'ultima versione di Newtonsoft.Json.Schema
        // e di riferire correttamente lo spazio dei nomi se usi ValidationError o altri tipi specifici.
        private static void PerformValidation(string json, string schemaJson)
        {
            JSchema schema = JSchema.Parse(schemaJson);
            JObject jsonObject = JObject.Parse(json);

            // Qui specifico esplicitamente quale overload utilizzare
            IList<ValidationError> validationErrors; // Assicurati di riferire lo spazio dei nomi corretto per ValidationError
            bool isValid = jsonObject.IsValid(schema, out validationErrors);

            if (!isValid)
            {
                // Esempio di come potresti voler gestire o trasformare gli oggetti ValidationError in stringhe
                var errorMessages = validationErrors.Select(e => e.Message);
                throw new JsonException($"Validazione fallita. Errori: {string.Join(", ", errorMessages)}");
            }
        }


        /// <summary>
        /// Crea un'istanza di un dato tipo utilizzando AutoFixture.
        /// </summary>
        /// <param name="type">Il tipo per cui generare un'istanza.</param>
        /// <returns>Un'istanza dell'oggetto del tipo specificato.</returns>
        private static object CreateInstance(Type type)
        {
            var context = new SpecimenContext(_fixture as ISpecimenBuilder);
            return context.Resolve(type) ?? throw new InvalidOperationException($"Impossibile creare un'istanza del tipo {type.FullName}.");
        }
    }
}
