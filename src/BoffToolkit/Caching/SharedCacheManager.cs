using System;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Manages a cache of objects of type <typeparamref name="TValue"/> indexed by keys of type <typeparamref name="TKey"/>, implementing the singleton pattern.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used in the cache.</typeparam>
    /// <typeparam name="TValue">The type of objects stored in the cache.</typeparam>
    public class SharedCacheManager<TKey, TValue> : CacheManager<TKey, TValue> where TKey : notnull {
        // Lazy instance of the shared cache manager.
        private static readonly Lazy<SharedCacheManager<TKey, TValue>> _instance =
            new(() => new SharedCacheManager<TKey, TValue>());

        /// <summary>
        /// Gets the shared instance of <see cref="SharedCacheManager{TKey, TValue}"/>.
        /// </summary>
        public static SharedCacheManager<TKey, TValue> Instance => _instance.Value;

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private SharedCacheManager() { }
    }
}