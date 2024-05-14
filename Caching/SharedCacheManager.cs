using System;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Gestisce una cache di oggetti di tipo TValue indicizzata da chiavi di tipo TKey, implementando il pattern singleton.
    /// </summary>
    /// <typeparam name="TKey">Il tipo delle chiavi nella cache.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nella cache.</typeparam>
    public class SharedCacheManager<TKey, TValue> : CacheManager<TKey, TValue> where TKey : notnull {
        private static readonly Lazy<SharedCacheManager<TKey, TValue>> _instance =
            new Lazy<SharedCacheManager<TKey, TValue>>(() => new SharedCacheManager<TKey, TValue>());

        /// <summary>
        /// Ottiene l'istanza condivisa di SharedCacheManager.
        /// </summary>
        public static SharedCacheManager<TKey, TValue> Instance => _instance.Value;

        /// <summary>
        /// Costruttore privato per evitare la creazione di istanze esterne.
        /// </summary>
        private SharedCacheManager() { }
    }
}
