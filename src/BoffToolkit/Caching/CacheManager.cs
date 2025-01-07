using System.Collections.Concurrent;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Manages a cache of objects of type <typeparamref name="TValue"/> indexed by keys of type <typeparamref name="TKey"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used in the cache.</typeparam>
    /// <typeparam name="TValue">The type of objects stored in the cache.</typeparam>
    public class CacheManager<TKey, TValue> where TKey : notnull {
        // Internal cache storing key-value pairs.
        private readonly ConcurrentDictionary<TKey, TValue?> _cache = new();

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the value to retrieve from the cache.</param>
        /// <returns>The value associated with the key if present; otherwise, the default value (null for reference types).</returns>
        public TValue? TryGetValue(TKey key) {
            _cache.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Retrieves or generates the value associated with the given key.
        /// If the value is not present in the cache, it generates the value using the provided function.
        /// </summary>
        /// <param name="key">The key to access or generate the value in the cache.</param>
        /// <param name="valueProvider">The function that generates the value if it is not present in the cache.</param>
        /// <param name="allowNullValues">Specifies whether null values should be accepted in the cache.</param>
        /// <returns>The value associated with the given key, which may be null.</returns>
        public TValue? GetOrProvide(TKey key, Func<TKey, TValue?> valueProvider, bool allowNullValues = true) {
            return GetOrProvideInternal(key, () => valueProvider(key), allowNullValues);
        }

        /// <summary>
        /// Retrieves or generates the value associated with the given key without additional parameters.
        /// If the value is not present in the cache, it generates the value using the provided function.
        /// </summary>
        /// <param name="key">The key to access or generate the value in the cache.</param>
        /// <param name="valueProvider">The function that generates the value if it is not present in the cache.</param>
        /// <param name="allowNullValues">Specifies whether null values should be accepted in the cache.</param>
        /// <returns>The value associated with the given key, which may be null.</returns>
        public TValue? GetOrProvide(TKey key, Func<TValue?> valueProvider, bool allowNullValues = false) {
            return GetOrProvideInternal(key, valueProvider, allowNullValues);
        }

        /// <summary>
        /// Removes the item associated with the specified key from the cache.
        /// </summary>
        /// <param name="key">The key of the item to remove from the cache.</param>
        /// <returns><c>true</c> if the item was successfully removed; otherwise, <c>false</c>.</returns>
        public bool Remove(TKey key) {
            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// Internal logic for retrieving or generating the value associated with the given key.
        /// </summary>
        /// <param name="key">The key to access or generate the value in the cache.</param>
        /// <param name="valueProvider">The function that generates the value if it is not present in the cache.</param>
        /// <param name="allowNullValues">Specifies whether null values should be accepted in the cache.</param>
        /// <returns>The value associated with the given key, which may be null.</returns>
        private TValue? GetOrProvideInternal(TKey key, Func<TValue?> valueProvider, bool allowNullValues) {
            if (!_cache.TryGetValue(key, out var cachedValue)) {
                var value = valueProvider();

                if (!Equals(value, default) || allowNullValues) {
                    _cache.TryAdd(key, value);
                }

                return value;
            }

            return cachedValue;
        }
    }
}