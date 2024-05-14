using System.Collections.Concurrent;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Gestisce una cache di oggetti di tipo TValue indicizzata da chiavi di tipo TKey.
    /// </summary>
    /// <typeparam name="TKey">Il tipo delle chiavi nella cache.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nella cache.</typeparam>
    public class CacheManager<TKey, TValue> where TKey : notnull {
        // Cache interna che memorizza coppie chiave-valore.
        private readonly ConcurrentDictionary<TKey, TValue?> _cache = new ConcurrentDictionary<TKey, TValue?>();

        /// <summary>
        /// Prova a ottenere il valore dalla cache associato alla chiave specificata.
        /// </summary>
        /// <param name="key">La chiave del valore da cercare nella cache.</param>
        /// <returns>Il valore associato alla chiave se presente, altrimenti default (null per i tipi di riferimento).</returns>
        public TValue? TryGetValue(TKey key) {
            _cache.TryGetValue(key, out TValue? value);
            return value;
        }

        /// <summary>
        /// Ottiene o genera il valore associato alla chiave fornita.
        /// Se il valore non è presente nella cache, lo genera utilizzando la funzione valueProvider.
        /// </summary>
        /// <param name="key">La chiave per accedere o generare il valore nella cache.</param>
        /// <param name="valueProvider">La funzione che genera il valore se non è presente nella cache.</param>
        /// <param name="allowNullValues">Specifica se i valori null devono essere accettati nella cache.</param>
        /// <returns>Il valore associato alla chiave fornita, può essere null.</returns>
        public TValue? GetOrProvide(TKey key, Func<TKey, TValue?> valueProvider, bool allowNullValues = true) {
            // Prova a ottenere il valore dalla cache.
            if (!_cache.TryGetValue(key, out TValue? cachedValue)) {
                // Genera il valore se non presente nella cache.
                var value = valueProvider(key);

                // Aggiunge il valore alla cache se non è null o se i valori null sono consentiti.
                if (value != null || allowNullValues) {
                    _cache.TryAdd(key, value);
                }

                return value;
            }

            // Restituisce il valore dalla cache se già presente.
            return cachedValue;
        }

        /// <summary>
        /// Ottiene o genera il valore associato alla chiave fornita senza utilizzare parametri aggiuntivi.
        /// Se il valore non è presente nella cache, lo genera utilizzando la funzione valueProvider.
        /// </summary>
        /// <param name="key">La chiave per accedere o generare il valore nella cache.</param>
        /// <param name="valueProvider">La funzione che genera il valore se non è presente nella cache.</param>
        /// <param name="allowNullValues">Specifica se i valori null devono essere accettati nella cache.</param>
        /// <returns>Il valore associato alla chiave fornita, può essere null.</returns>
        public TValue? GetOrProvide(TKey key, Func<TValue?> valueProvider, bool allowNullValues = false) {
            // Prova a ottenere il valore dalla cache.
            if (!_cache.TryGetValue(key, out TValue? cachedValue)) {
                // Genera il valore se non presente nella cache.
                var value = valueProvider();

                // Aggiunge il valore alla cache se non è null o se i valori null sono consentiti.
                if (value != null || allowNullValues) {
                    _cache.TryAdd(key, value);
                }

                return value;
            }

            // Restituisce il valore dalla cache se già presente.
            return cachedValue;
        }

        /// <summary>
        /// Rimuove l'elemento associato alla chiave specificata dalla cache.
        /// </summary>
        /// <param name="key">La chiave dell'elemento da rimuovere dalla cache.</param>
        /// <returns>true se l'elemento è stato rimosso correttamente; in caso contrario, false.</returns>
        public bool Remove(TKey key) {
            return _cache.TryRemove(key, out _);
        }
    }
}
