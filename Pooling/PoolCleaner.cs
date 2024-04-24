using System.Collections.Concurrent;

namespace BoffToolkit.Pooling
{
    /// <summary>
    /// Gestisce la pulizia periodica del pool di oggetti in base al tempo trascorso dall'ultimo utilizzo.
    /// </summary>
    /// <typeparam name="TKey">Il tipo della chiave utilizzata per identificare ogni pool.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nei pool.</typeparam>
    internal class PoolCleaner<TKey, TValue>
        where TKey : notnull
        where TValue : class, IPoolable<TValue>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool;
        private TimeSpan _cleanupInterval;
        private TimeSpan _maxIdleTime;
        private readonly Timer _cleanupTimer;

        public PoolCleaner(ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> pool, TimeSpan? cleanupInterval, TimeSpan? maxIdleTime)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _cleanupInterval = cleanupInterval ?? TimeSpan.FromMinutes(30);
            _maxIdleTime = maxIdleTime ?? TimeSpan.FromHours(1);

            _cleanupTimer = new Timer(CleanupPool, null, _cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Aggiorna le impostazioni di intervallo di pulizia e tempo massimo di inattività.
        /// </summary>
        /// <param name="newCleanupInterval">Il nuovo intervallo di tempo tra le pulizie periodiche.</param>
        /// <param name="newMaxIdleTime">Il nuovo tempo massimo di inattività prima che un oggetto venga rimosso.</param>
        public void UpdateSettings(TimeSpan newCleanupInterval, TimeSpan newMaxIdleTime)
        {
            _cleanupInterval = newCleanupInterval;
            _maxIdleTime = newMaxIdleTime;
            
            // Aggiorna il timer per usare il nuovo intervallo di pulizia
            _cleanupTimer.Change(_cleanupInterval, _cleanupInterval);
        }

        private void CleanupPool(object? state)
        {
            var currentTime = DateTime.UtcNow;

            foreach (var key in _pool.Keys)
            {
                if (_pool.TryGetValue(key, out var queue))
                {
                    var itemsToRemove = new ConcurrentQueue<TValue>();

                    foreach (var instance in queue)
                    {
                        if (currentTime - instance.LastUsedTime > _maxIdleTime)
                        {
                            itemsToRemove.Enqueue(instance);
                        }
                    }

                    while (!itemsToRemove.IsEmpty)
                    {
                        if (itemsToRemove.TryDequeue(out var removedItem))
                        {
                            removedItem.Dispose(); // Assicurati che Cleanup() sia implementato correttamente
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Avvia il processo di pulizia manuale.
        /// </summary>
        public void StartCleaning()
        {
            _cleanupTimer.Change(TimeSpan.Zero, _cleanupInterval);
        }

        /// <summary>
        /// Ferma il processo di pulizia manuale.
        /// </summary>
        public void StopCleaning()
        {
            _cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
