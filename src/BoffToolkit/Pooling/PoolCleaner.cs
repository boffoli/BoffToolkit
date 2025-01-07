using System.Collections.Concurrent;

namespace BoffToolkit.Pooling {
    /// <summary>
    /// Manages periodic cleanup of object pools based on the elapsed time since their last usage.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify each pool.</typeparam>
    /// <typeparam name="TValue">The type of objects stored in the pools, which must implement <see cref="IPoolable"/>.</typeparam>
    internal class PoolCleaner<TKey, TValue>
        where TKey : notnull
        where TValue : class, IPoolable {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool;
        private TimeSpan _cleanupInterval;
        private TimeSpan _maxIdleTime;
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolCleaner{TKey, TValue}"/> class with the specified settings.
        /// </summary>
        /// <param name="pool">The dictionary containing the object pools to clean.</param>
        /// <param name="cleanupInterval">The interval between periodic cleanup operations.</param>
        /// <param name="maxIdleTime">The maximum idle time before an object is removed from the pool.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="pool"/>, <paramref name="cleanupInterval"/>, or <paramref name="maxIdleTime"/> is null or zero.
        /// </exception>
        public PoolCleaner(ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> pool, TimeSpan cleanupInterval, TimeSpan maxIdleTime) {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _cleanupInterval = cleanupInterval == TimeSpan.Zero ? throw new ArgumentNullException(nameof(cleanupInterval)) : cleanupInterval;
            _maxIdleTime = maxIdleTime == TimeSpan.Zero ? throw new ArgumentNullException(nameof(maxIdleTime)) : maxIdleTime;

            _cleanupTimer = new Timer(CleanupPool, null, _cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Updates the cleanup interval and maximum idle time settings.
        /// </summary>
        /// <param name="newCleanupInterval">The new interval between cleanup operations.</param>
        /// <param name="newMaxIdleTime">The new maximum idle time before an object is removed.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="newCleanupInterval"/> or <paramref name="newMaxIdleTime"/> is null or zero.
        /// </exception>
        public void UpdateSettings(TimeSpan newCleanupInterval, TimeSpan newMaxIdleTime) {
            _cleanupInterval = newCleanupInterval == TimeSpan.Zero ? throw new ArgumentNullException(nameof(newCleanupInterval)) : newCleanupInterval;
            _maxIdleTime = newMaxIdleTime == TimeSpan.Zero ? throw new ArgumentNullException(nameof(newMaxIdleTime)) : newMaxIdleTime;

            // Update the timer with the new cleanup interval
            _cleanupTimer.Change(_cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Starts the manual cleanup process immediately.
        /// </summary>
        public void StartCleaning() {
            _cleanupTimer.Change(TimeSpan.Zero, _cleanupInterval);
        }

        /// <summary>
        /// Stops the manual cleanup process.
        /// </summary>
        public void StopCleaning() {
            _cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Cleans up the object pool by removing items that have exceeded the maximum idle time.
        /// </summary>
        /// <param name="state">The state object passed by the timer.</param>
        private void CleanupPool(object? state) {
            var currentTime = DateTime.UtcNow;

            foreach (var key in _pool.Keys) {
                if (_pool.TryGetValue(key, out var queue)) {
                    var itemsToKeep = new ConcurrentQueue<TValue>();

                    while (queue.TryDequeue(out var instance)) {
                        if (currentTime - instance.LastUsedTime > _maxIdleTime) {
                            instance.DisposeAsync().AsTask().Wait();
                        }
                        else {
                            itemsToKeep.Enqueue(instance);
                        }
                    }

                    // Replace the original queue with the new one containing only non-expired items
                    _pool[key] = itemsToKeep;
                }
            }
        }
    }
}