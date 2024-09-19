using AutoFixture;
using AutoFixture.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace BoffToolkit.JsonValidator {
    /// <summary>
    /// Fornisce funzionalità per la validazione di contenuti JSON contro uno schema JSON.
    /// </summary>
    /// <remarks>
    /// Inizializza una nuova istanza della classe SchemaValidator con uno schema JSON specificato.
    /// </remarks>
    /// <param name="jsonSchema">Lo schema JSON da utilizzare per la validazione.</param>
    public class SchemaValidator(string jsonSchema) {
        private readonly string _jsonSchema = jsonSchema ?? throw new ArgumentNullException(nameof(jsonSchema), "Lo schema JSON è necessario.");
        private static readonly Fixture _fixture = new();

        /// <summary>
        /// Valida una stringa JSON contro lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        public void Validate(string jsonContent) => ValidationHelper.ValidateContent(jsonContent, _jsonSchema);

        /// <summary>
        /// Valida un oggetto serializzandolo in JSON e confrontandolo con lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="instance">L'istanza dell'oggetto da validare.</param>
        public void Validate(object instance) => ValidationHelper.ValidateContent(JsonConvert.SerializeObject(instance), _jsonSchema);

        /// <summary>
        /// Valida un tipo generando un'istanza e confrontando la sua serializzazione JSON con lo schema JSON fornito all'inizializzazione.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto da validare.</param>
        public void Validate(Type type) => ValidationHelper.ValidateContent(SerializeInstance(type), _jsonSchema);

        /// <summary>
        /// Prova a validare una stringa JSON contro lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(string jsonContent) => ValidationHelper.TryValidateContent(jsonContent, _jsonSchema);

        /// <summary>
        /// Prova a validare un'istanza di oggetto serializzandola in JSON e confrontandola con lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(object instance) => ValidationHelper.TryValidateContent(JsonConvert.SerializeObject(instance), _jsonSchema);

        /// <summary>
        /// Prova a validare un tipo generando un'istanza e confrontando la sua serializzazione JSON con lo schema JSON fornito, ritornando un oggetto ValidationResult.
        /// </summary>
        /// <param name="type">Il tipo da validare.</param>
        /// <returns>Un oggetto ValidationResult che indica se la validazione è riuscita o meno.</returns>
        public ValidationResult TryValidate(Type type) => ValidationHelper.TryValidateContent(SerializeInstance(type), _jsonSchema);

        // Metodi statici pubblici

        /// <summary>
        /// Valida una stringa JSON contro uno schema JSON specificato.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(string jsonContent, string jsonSchema) => ValidationHelper.ValidateContent(jsonContent, jsonSchema);

        /// <summary>
        /// Valida un'istanza di oggetto serializzandola in JSON e confrontandola con uno schema JSON specificato.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(object instance, string jsonSchema) => ValidationHelper.ValidateContent(JsonConvert.SerializeObject(instance), jsonSchema);

        /// <summary>
        /// Valida un tipo generando un'istanza e confrontando la sua serializzazione JSON con uno schema JSON specificato.
        /// </summary>
        /// <param name="type">Il tipo da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        public static void Validate(Type type, string jsonSchema) => ValidationHelper.ValidateContent(SerializeInstance(type), jsonSchema);

        /// <summary>
        /// Prova a validare una stringa JSON rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="jsonContent">La stringa JSON da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(string jsonContent, string jsonSchema) => ValidationHelper.TryValidateContent(jsonContent, jsonSchema);

        /// <summary>
        /// Prova a validare un'istanza di un oggetto rispetto a uno schema JSON fornito, convertendolo prima in una stringa JSON.
        /// </summary>
        /// <param name="instance">L'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(object instance, string jsonSchema) => ValidationHelper.TryValidateContent(JsonConvert.SerializeObject(instance), jsonSchema);

        /// <summary>
        /// Prova a validare un tipo generando un'istanza e convertendola in una stringa JSON rispetto a uno schema JSON fornito.
        /// </summary>
        /// <param name="type">Il tipo dell'oggetto da validare.</param>
        /// <param name="jsonSchema">Lo schema JSON contro cui effettuare la validazione.</param>
        /// <returns>Un oggetto <see cref="ValidationResult"/> che indica se la validazione è riuscita e contiene gli eventuali messaggi di errore.</returns>
        public static ValidationResult TryValidate(Type type, string jsonSchema) => ValidationHelper.TryValidateContent(SerializeInstance(type), jsonSchema);

        // Metodi privati statici
        private static string SerializeInstance(Type type) {
            var context = new SpecimenContext(_fixture as ISpecimenBuilder);
            var instance = context.Resolve(type);
            return JsonConvert.SerializeObject(instance);
        }

        // Classe helper interna per la logica di validazione
        private static class ValidationHelper {
            public static void ValidateContent(string jsonContent, string jsonSchema) {
                var result = TryValidateContent(jsonContent, jsonSchema);
                if (!result.IsValid) {
                    throw new JsonException($"Validazione fallita. Errori: {string.Join(", ", result.ErrorMessages)}");
                }
            }

            public static ValidationResult TryValidateContent(string jsonContent, string jsonSchema) {
                try {
                    Console.WriteLine("Schema JSON:");
                    Console.WriteLine(jsonSchema); // Stampa lo schema JSON
                    var schema = JSchema.Parse(jsonSchema);

                    Console.WriteLine("Contenuto JSON:");
                    Console.WriteLine(jsonContent); // Stampa il contenuto JSON
                    var token = JToken.Parse(jsonContent);
                    var isValid = token.IsValid(schema, out IList<ValidationError> validationErrors);

                    var errorMessages = validationErrors.Select(e => e.Message).ToList();
                    return new ValidationResult(isValid, errorMessages);
                }
                catch (JsonReaderException ex) {
                    return new ValidationResult(false, new List<string> { $"Errore di parsing JSON: {ex.Message}" });
                }
                catch (JSchemaException ex) {
                    return new ValidationResult(false, new List<string> { $"Errore nello schema JSON: {ex.Message}" });
                }
                catch (Exception ex) {
                    return new ValidationResult(false, new List<string> { $"Errore sconosciuto durante la validazione: {ex.Message}" });
                }
            }
        }
    }

    /// <summary>
    /// Rappresenta il risultato di una validazione, includendo lo stato di validità e gli eventuali messaggi di errore.
    /// </summary>
    public class ValidationResult(bool isValid, IList<string> errorMessages) {
        public bool IsValid { get; } = isValid;
        public IList<string> ErrorMessages { get; } = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages), "I messaggi di errore sono necessari.");
    }
}