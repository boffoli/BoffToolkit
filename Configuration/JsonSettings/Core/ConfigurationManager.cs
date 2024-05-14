using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BoffToolkit.Configuration.JsonSettings.Core {
    /// <summary>
    /// Gestisce la configurazione basata su file JSON.
    /// </summary>
    internal class ConfigurationManager {
        private readonly ConfigurationBuilder _builder = new ConfigurationBuilder();
        private IConfiguration? _cachedConfiguration;

        /// <summary>
        /// Aggiunge la configurazione da un flusso JSON.
        /// </summary>
        /// <param name="jsonContent">Il contenuto JSON da aggiungere alla configurazione.</param>
        public void AddJsonStream(string jsonContent) {
            _builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)));
        }

        /// <summary>
        /// Aggiunge la configurazione da un file JSON.
        /// </summary>
        /// <param name="configFilePath">Il percorso del file JSON di configurazione.</param>
        public void AddJsonFile(string configFilePath) {
            _builder.AddJsonFile(configFilePath, optional: false, reloadOnChange: true);
        }

        /// <summary>
        /// Costruisce la configurazione.
        /// </summary>
        /// <returns>La configurazione costruita.</returns>
        public IConfiguration Build() {
            if (_cachedConfiguration == null) {
                _cachedConfiguration = _builder.Build();
            }
            return _cachedConfiguration;
        }

        /// <summary>
        /// Stampa la configurazione come stringa JSON.
        /// </summary>
        /// <exception cref="InvalidOperationException">Viene generata quando la configurazione non è ancora stata costruita.</exception>
        public void PrintConfigAsJson() {
            if (_cachedConfiguration == null) {
                throw new InvalidOperationException("La configurazione non è stata ancora costruita.");
            }

            var configData = new Dictionary<string, string>();
            foreach (var section in _cachedConfiguration.AsEnumerable()) {
                if (section.Value != null) {
                    configData.Add(section.Key, section.Value);
                }
            }

            string json = JsonConvert.SerializeObject(configData, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
