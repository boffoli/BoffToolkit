using System.Collections.Concurrent;

namespace BoffToolkit.Pooling {
    /// <summary>
    /// Gestisce la pulizia periodica del pool di oggetti in base al tempo trascorso dall'ultimo utilizzo.
    /// </summary>
    /// <typeparam name="TKey">Il tipo della chiave utilizzata per identificare ogni pool.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nei pool.</typeparam>
    internal class PoolCleaner<TKey, TValue>
        where TKey : notnull
        where TValue : class, IPoolable<TValue> {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool;
        private TimeSpan _cleanupInterval;
        private TimeSpan _maxIdleTime;
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="PoolCleaner{TKey, TValue}"/> con le impostazioni specificate.
        /// </summary>
        /// <param name="pool">Il dizionario contenente i pool di oggetti da pulire.</param>
        /// <param name="cleanupInterval">L'intervallo di tempo tra le pulizie periodiche.</param>
        /// <param name="maxIdleTime">Il tempo massimo di inattività prima che un oggetto venga rimosso.</param>
        /// <exception cref="ArgumentNullException">Sollevata se <paramref name="pool"/>, <paramref name="cleanupInterval"/> o <paramref name="maxIdleTime"/> è null o zero.</exception>
        public PoolCleaner(ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> pool, TimeSpan cleanupInterval, TimeSpan maxIdleTime) {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _cleanupInterval = cleanupInterval == TimeSpan.Zero ? throw new ArgumentNullException(nameof(cleanupInterval)) : cleanupInterval;
            _maxIdleTime = maxIdleTime == TimeSpan.Zero ? throw new ArgumentNullException(nameof(maxIdleTime)) : maxIdleTime;

            _cleanupTimer = new Timer(CleanupPool, null, _cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Aggiorna le impostazioni di intervallo di pulizia e tempo massimo di inattività.
        /// </summary>
        /// <param name="newCleanupInterval">Il nuovo intervallo di tempo tra le pulizie periodiche.</param>
        /// <param name="newMaxIdleTime">Il nuovo tempo massimo di inattività prima che un oggetto venga rimosso.</param>
        /// <exception cref="ArgumentNullException">Sollevata se <paramref name="newCleanupInterval"/> o <paramref name="newMaxIdleTime"/> è null o zero.</exception>
        public void UpdateSettings(TimeSpan newCleanupInterval, TimeSpan newMaxIdleTime) {
            _cleanupInterval = newCleanupInterval == TimeSpan.Zero ? throw new ArgumentNullException(nameof(newCleanupInterval)) : newCleanupInterval;
            _maxIdleTime = newMaxIdleTime == TimeSpan.Zero ? throw new ArgumentNullException(nameof(newMaxIdleTime)) : newMaxIdleTime;

            // Aggiorna il timer per usare il nuovo intervallo di pulizia
            _cleanupTimer.Change(_cleanupInterval, _cleanupInterval);
        }

        /// <summary>
        /// Avvia il processo di pulizia manuale.
        /// </summary>
        public void StartCleaning() {
            _cleanupTimer.Change(TimeSpan.Zero, _cleanupInterval);
        }

        /// <summary>
        /// Ferma il processo di pulizia manuale.
        /// </summary>
        public void StopCleaning() {
            _cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Esegue la pulizia del pool di oggetti rimuovendo gli elementi che hanno superato il tempo massimo di inattività.
        /// </summary>
        /// <param name="state">Stato dell'oggetto passato dal timer.</param>
        private void CleanupPool(object? state) {
            var currentTime = DateTime.UtcNow;

            foreach (var key in _pool.Keys) {
                if (_pool.TryGetValue(key, out var queue)) {
                    var itemsToRemove = new ConcurrentQueue<TValue>();
                    var itemsToKeep = new ConcurrentQueue<TValue>();

                    while (queue.TryDequeue(out var instance)) {
                        if (currentTime - instance.LastUsedTime > _maxIdleTime) {
                            itemsToRemove.Enqueue(instance);
                        }
                        else {
                            itemsToKeep.Enqueue(instance);
                        }
                    }

                    // Sostituisci la coda originale con la nuova coda che contiene solo gli elementi non scaduti
                    _pool[key] = itemsToKeep;

                    // Dispone gli elementi scaduti
                    while (!itemsToRemove.IsEmpty) {
                        if (itemsToRemove.TryDequeue(out var removedItem)) {
                            removedItem.DisposeAsync().AsTask().Wait(); // Assicurati che Cleanup() sia implementato correttamente
                        }
                    }
                }
            }
        }

    }
}
