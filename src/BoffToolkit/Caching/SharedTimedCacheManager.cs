using System;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Manages a cache of objects of type <typeparamref name="TValue"/> indexed by keys of type <typeparamref name="TKey"/>, with timed expiration, implementing the singleton pattern.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used in the cache.</typeparam>
    /// <typeparam name="TValue">The type of objects stored in the cache.</typeparam>
    public class SharedTimedCacheManager<TKey, TValue> : TimedCacheManager<TKey, TValue> where TKey : notnull {
        // Lazy instance of the shared timed cache manager.
        private static Lazy<SharedTimedCacheManager<TKey, TValue>>? _lazyInstance;

        /// <summary>
        /// Gets the shared instance of <see cref="SharedTimedCacheManager{TKey, TValue}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the shared instance has not been configured using the <see cref="Configure"/> method.
        /// </exception>
        public static SharedTimedCacheManager<TKey, TValue> Instance =>
            _lazyInstance?.Value ?? throw new InvalidOperationException("The instance of SharedTimedCacheManager has not been configured.");

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        /// <param name="timerInterval">The interval between cache checks.</param>
        /// <param name="timerAction">The action to execute when the timer expires.</param>
        private SharedTimedCacheManager(TimeSpan timerInterval, Action timerAction)
            : base(timerInterval, timerAction) {
        }

        /// <summary>
        /// Configures the shared instance of <see cref="SharedTimedCacheManager{TKey, TValue}"/> with the specified timer interval and action.
        /// </summary>
        /// <param name="timerInterval">The interval between cache checks.</param>
        /// <param name="timerAction">The action to execute when the timer expires.</param>
        public static void Configure(TimeSpan timerInterval, Action timerAction) {
            _lazyInstance ??= new Lazy<SharedTimedCacheManager<TKey, TValue>>(
                () => new SharedTimedCacheManager<TKey, TValue>(timerInterval, timerAction));
        }
    }
}