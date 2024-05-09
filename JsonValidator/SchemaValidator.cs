using AutoFixture;
using AutoFixture.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace BoffToolkit.JsonValidator
{
    /// <summary>
    /// Fornisce funzionalità per la validazione di contenuti JSON contro uno schema JSON.
    /// </summary>
    public class SchemaValidator
    {
        private readonly string _jsonSchema;
        private static readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// Inizializza una nuova istanza della classe SchemaValidator con uno schema JSON specificato.
        /// </summary>
        /// <param name="jsonSchema">Lo schema JSON da utilizzare per la validazione.</param>
        public SchemaValidator(string jsonSchema)
        {
            _jsonSchema = jsonSchema ?? throw new ArgumentNullException(nameof(jsonSchema), "Lo schema JSON è necessario.");
        }

        /// <summary>
        /// Valida una stringa JSON contro lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        public void Validate(string jsonContent) => Helper.Validate(jsonContent, _jsonSchema);

        /// <summary>
        /// Valida un oggetto serializzandolo in JSON e confrontandolo con lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="instance">L'istanza dell'oggetto da validare.</param>
        public void Validate(object instance) => Helper.Validate(JsonConvert.SerializeObject(instance), _jsonSchema);

        /// <summary>
        /// Valida un tipo generando un'istanza e confrontando la sua serializzazione JSON con lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto da validare.</param>
        public void Validate(Type type) => Helper.Validate(SerializeInstance(type), _jsonSchema);

        /// <summary>
        /// Prova a validare una stringa JSON contro lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(string jsonContent) => Helper.TryValidate(jsonContent, _jsonSchema);

        /// <summary>
        /// Prova a validare un'istanza di oggetto serializzandola in JSON e confrontandola con lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(object instance) => Helper.TryValidate(JsonConvert.SerializeObject(instance), _jsonSchema);

        /// <summary>
        /// Prova a validare un tipo generando un'istanza e confrontando la sua serializzazione JSON con lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="type">Il tipo da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(Type type) => Helper.TryValidate(SerializeInstance(type), _jsonSchema);

        // Metodi statici pubblici

        /// <summary>
        /// Valida una stringa JSON contro uno schema JSON specificato.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(string jsonContent, string jsonSchema) => Helper.Validate(jsonContent, jsonSchema);

        /// <summary>
        /// Valida un'istanza di oggetto serializzandola in JSON e confrontandola con uno schema JSON specificato.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(object instance, string jsonSchema) => Helper.Validate(JsonConvert.SerializeObject(instance), jsonSchema);

        /// <summary>
        /// Valida un tipo generando un'istanza e confrontando la sua serializzazione JSON con uno schema JSON specificato.
        /// </summary>
        /// <param name="type">Il tipo da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(Type type, string jsonSchema) => Helper.Validate(SerializeInstance(type), jsonSchema);

        /// <summary>
        /// Prova a validare una stringa JSON rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(string jsonContent, string jsonSchema) => Helper.TryValidate(jsonContent, jsonSchema);

        /// <summary>
        /// Prova a validare un'istanza di un oggetto rispetto a uno schema JSON fornito, convertendolo prima in una stringa JSON.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(object instance, string jsonSchema) => Helper.TryValidate(JsonConvert.SerializeObject(instance), jsonSchema);

        /// <summary>
        /// Prova a validare un tipo generando un'istanza e convertendola in una stringa JSON rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(Type type, string jsonSchema) => Helper.TryValidate(SerializeInstance(type), jsonSchema);

        // Metodi privati statici
        private static string SerializeInstance(Type type)
        {
            var context = new SpecimenContext(_fixture as ISpecimenBuilder);
            object instance = context.Resolve(type);
            return JsonConvert.SerializeObject(instance);
        }

        // Classe helper interna per la logica di validazione
        private static class Helper
        {
            public static void Validate(string jsonContent, string jsonSchema)
            {
                var result = TryValidate(jsonContent, jsonSchema);
                if (!result.IsValid)
                {
                    throw new JsonException($"Validazione fallita. Errori: {string.Join(", ", result.ErrorMessages)}");
                }
            }

            public static ValidationResult TryValidate(string jsonContent, string jsonSchema)
            {
                JSchema schema = JSchema.Parse(jsonSchema);
                JToken token = JToken.Parse(jsonContent);
                bool isValid = token.IsValid(schema, out IList<ValidationError> validationErrors);

                var errorMessages = validationErrors.Select(e => e.Message).ToList();
                return new ValidationResult(isValid, errorMessages);
            }
        }
    }

    /// <summary>
    /// Rappresenta il risultato di una validazione, includendo lo stato di validità e gli eventuali messaggi di errore.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; }
        public IList<string> ErrorMessages { get; }

        public ValidationResult(bool isValid, IList<string> errorMessages)
        {
            IsValid = isValid;
            ErrorMessages = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages), "I messaggi di errore sono necessari.");
        }
    }
}
