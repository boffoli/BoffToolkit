using System.Reflection;

namespace BoffToolkit.TypeLoading
{
    /// <summary>
    /// Fornisce un meccanismo per caricare e manipolare tipi da assembly.
    /// </summary>
    public class DynamicTypeLoader
    {
        /// <summary>
        /// Carica un tipo da un nome completo qualificato.
        /// </summary>
        /// <param name="fullyQualifiedName">Nome completo del tipo da caricare.</param>
        /// <param name="assemblyPath">Percorso opzionale dell'assembly esterno da cui caricare il tipo. Usa l'assembly corrente se non specificato.</param>
        /// <returns>Il tipo caricato dall'assembly specificato.</returns>
        /// <exception cref="ArgumentException">Lanciata se il nome completo qualificato del tipo è null o vuoto.</exception>
        /// <exception cref="InvalidOperationException">Se il tipo non può essere trovato nell'assembly specificato.</exception>
        public static Type LoadType(string fullyQualifiedName, string? assemblyPath = null)
        {
            if (string.IsNullOrEmpty(fullyQualifiedName))
            {
                throw new ArgumentException("Il nome completo qualificato del tipo non può essere null o vuoto.", nameof(fullyQualifiedName));
            }

            Type? type = null;

            // Carica il tipo dall'assembly specificato se viene fornito un percorso.
            if (!string.IsNullOrEmpty(assemblyPath))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                type = assembly.GetType(fullyQualifiedName, throwOnError: false);
            }

            // Prova a caricare il tipo dall'assembly corrente se non è stato trovato e non è stato fornito un percorso di assembly.
            if (type == null)
            {
                type = Type.GetType(fullyQualifiedName);
            }

            // Lancia un'eccezione se il tipo non è stato trovato.
            if (type == null)
            {
                throw new InvalidOperationException($"Tipo {fullyQualifiedName} non trovato.");
            }

            return type;
        }

        // Il metodo BindGenericParameters è stato commentato nell'implementazione originale.
        // Se desideri includerlo, decommentalo e documentalo come gli altri metodi.

        /*public static Type BindGenericParameters(Type genericType, Type[] typeSpecifiers)
        {
            // Implementazione del metodo
        }*/
    }
}
