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
        where TValue : class
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<PooledObject<TValue>>> _pool;
        private readonly TimeSpan _cleanupInterval;
        private readonly TimeSpan _maxIdleTime;
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// Inizializza una nuova istanza della classe PoolCleaner.
        /// </summary>
        /// <param name="pool">Il pool di oggetti da pulire.</param>
        /// <param name="cleanupInterval">Intervallo di tempo tra le pulizie periodiche.</param>
        /// <param name="maxIdleTime">Tempo massimo di inattività prima che un oggetto venga rimosso.</param>
        public PoolCleaner(ConcurrentDictionary<TKey, ConcurrentQueue<PooledObject<TValue>>> pool, TimeSpan? cleanupInterval, TimeSpan? maxIdleTime)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _cleanupInterval = cleanupInterval ?? TimeSpan.FromMinutes(5); // Intervallo predefinito per le pulizie: 5 minuti
            _maxIdleTime = maxIdleTime ?? TimeSpan.FromHours(1); // Tempo massimo di inattività predefinito: 1 ora

            _cleanupTimer = new Timer(CleanupPool, null, _cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Avvia il processo di pulizia manuale.
        /// </summary>
        public void StartCleaning()
        {
            // Avvia il processo di pulizia manualmente se necessario
            _cleanupTimer.Change(TimeSpan.Zero, _cleanupInterval);
        }

        /// <summary>
        /// Ferma il processo di pulizia manuale.
        /// </summary>
        public void StopCleaning()
        {
            // Ferma il processo di pulizia manualmente se necessario
            _cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Esegue la pulizia periodica del pool.
        /// </summary>
        /// <param name="state">Stato del timer (non utilizzato).</param>
        private void CleanupPool(object? state)
        {
            var currentTime = DateTime.UtcNow;

            foreach (var key in _pool.Keys)
            {
                if (_pool.TryGetValue(key, out var queue))
                {
                    var itemsToRemove = new ConcurrentQueue<PooledObject<TValue>>();

                    // Identifica gli oggetti inutilizzati da rimuovere
                    foreach (var instance in queue)
                    {
                        if (currentTime - instance.LastUsedTime > _maxIdleTime)
                        {
                            itemsToRemove.Enqueue(instance);
                        }
                    }

                    // Rimuove gli oggetti inutilizzati dal pool
                    while (!itemsToRemove.IsEmpty)
                    {
                        if (itemsToRemove.TryDequeue(out var removedItem))
                        {
                            // Gestione del rilascio delle risorse dell'istanza rimossa
                        }
                    }
                }
            }
        }
    }
}
