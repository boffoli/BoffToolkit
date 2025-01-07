using System.Reflection;

namespace BoffToolkit.TypeLoading {
    /// <summary>
    /// Provides a mechanism for loading and manipulating types from assemblies.
    /// </summary>
    public static class DynamicTypeLoader {
        /// <summary>
        /// Loads a type by its fully qualified name.
        /// </summary>
        /// <param name="fullyQualifiedName">The fully qualified name of the type to load.</param>
        /// <param name="assemblyPath">
        /// An optional path to an external assembly from which to load the type. 
        /// If not specified, the current assembly will be used.
        /// </param>
        /// <returns>The type loaded from the specified assembly.</returns>
        /// <exception cref="ArgumentException">Thrown if the fully qualified name of the type is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the type cannot be found in the specified assembly.</exception>
        public static Type LoadType(string fullyQualifiedName, string? assemblyPath = null) {
            if (string.IsNullOrEmpty(fullyQualifiedName)) {
                throw new ArgumentException("The fully qualified name of the type cannot be null or empty.", nameof(fullyQualifiedName));
            }

            Type? type = null;

            // Load the type from the specified assembly if a path is provided.
            if (!string.IsNullOrEmpty(assemblyPath)) {
                var an = AssemblyName.GetAssemblyName(assemblyPath);
                var assembly = Assembly.Load(an);
                type = assembly.GetType(fullyQualifiedName, throwOnError: false);
            }

            // Attempt to load the type from the current assembly if not found and no assembly path is provided.
            if (type == null) {
                type = Type.GetType(fullyQualifiedName);
            }

            // Throw an exception if the type was not found.
            if (type == null) {
                throw new InvalidOperationException($"Type {fullyQualifiedName} not found.");
            }

            return type;
        }
    }
}