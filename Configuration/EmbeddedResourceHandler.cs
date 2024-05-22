using System.Reflection;

namespace BoffToolkit.Configuration {
    /// <summary>
    /// Classe di utilità per gestire le risorse incorporate in un assembly.
    /// </summary>
    public static class EmbeddedResourceHandler {
        /// <summary>
        /// Legge una risorsa come stringa da un assembly.
        /// </summary>
        /// <param name="assembly">L'assembly contenente la risorsa.</param>
        /// <param name="resourceNamePath">Il percorso della risorsa nell'assembly.</param>
        /// <returns>Il contenuto della risorsa come stringa.</returns>
        public static string ReadResourceAsString(Assembly assembly, string resourceNamePath) {
            // Legge la risorsa come stringa
            using var stream = assembly.GetManifestResourceStream(resourceNamePath) ?? throw new InvalidOperationException($"La risorsa {resourceNamePath} non è stata trovata nell'assembly.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Estrae una risorsa in un file.
        /// </summary>
        /// <param name="assembly">L'assembly contenente la risorsa.</param>
        /// <param name="resourceNamePath">Il percorso della risorsa nell'assembly.</param>
        /// <param name="createTemporaryFile">Se vero, crea un file temporaneo; altrimenti, crea il file nella directory temporanea.</param>
        /// <returns>Il percorso del file estratto.</returns>
        public static string ExtractResourceToFile(Assembly assembly, string resourceNamePath, bool createTemporaryFile = false) {
            // Estrae la risorsa in un file
            using var stream = assembly.GetManifestResourceStream(resourceNamePath) ?? throw new InvalidOperationException($"La risorsa {resourceNamePath} non è stata trovata nell'assembly.");

            var filePath = createTemporaryFile ? Path.GetTempFileName() : $"{Path.GetTempPath()}{resourceNamePath}";
            using (var fileStream = File.Create(filePath)) {
                stream.CopyTo(fileStream);
            }

            return filePath;
        }
    }
}
