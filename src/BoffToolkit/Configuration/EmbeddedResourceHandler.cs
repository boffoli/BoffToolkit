using System.Reflection;

namespace BoffToolkit.Configuration {
    /// <summary>
    /// Utility class for managing embedded resources in an assembly.
    /// </summary>
    public static class EmbeddedResourceHandler {
        /// <summary>
        /// Reads an embedded resource as a string from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="resourceNamePath">The path of the resource within the assembly.</param>
        /// <returns>The content of the resource as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the resource cannot be found in the assembly.</exception>
        public static string ReadResourceAsString(Assembly assembly, string resourceNamePath) {
            // Reads the resource as a string
            using var stream = assembly.GetManifestResourceStream(resourceNamePath)
                ?? throw new InvalidOperationException($"The resource {resourceNamePath} was not found in the assembly.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Extracts an embedded resource to a file.
        /// </summary>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="resourceNamePath">The path of the resource within the assembly.</param>
        /// <param name="createTemporaryFile">
        /// If <c>true</c>, creates a temporary file with a random name.
        /// Otherwise, creates the file in the temporary directory with the resource name.
        /// </param>
        /// <returns>The path to the extracted file.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the resource cannot be found in the assembly.</exception>
        public static string ExtractResourceToFile(Assembly assembly, string resourceNamePath, bool createTemporaryFile = false) {
            // Extracts the resource to a file
            using var stream = assembly.GetManifestResourceStream(resourceNamePath)
                ?? throw new InvalidOperationException($"The resource {resourceNamePath} was not found in the assembly.");

            var filePath = createTemporaryFile
                ? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
                : Path.Combine(Path.GetTempPath(), resourceNamePath);

            using (var fileStream = File.Create(filePath)) {
                stream.CopyTo(fileStream);
            }

            return filePath;
        }
    }
}