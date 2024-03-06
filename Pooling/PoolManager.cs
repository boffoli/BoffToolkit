using System.Collections.Concurrent;

namespace BoffToolkit.Pooling
{
    /// <summary>
    /// Gestisce un pool di oggetti per ogni chiave. Questa classe viene utilizzata per riutilizzare gli oggetti
    /// anziché crearne di nuovi ogni volta.
    /// </summary>
    /// <typeparam name="TKey">Il tipo della chiave utilizzata per identificare ogni pool.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nei pool.</typeparam>
    public class PoolManager<TKey, TValue>
        where TKey : notnull
        where TValue : class
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<PooledObject<TValue>>> _pool = new ConcurrentDictionary<TKey, ConcurrentQueue<PooledObject<TValue>>>();
        private readonly Func<TKey, TValue> _instanceCreator;
        private readonly int? _maxInstancesPerKey;
        private readonly PoolCleaner<TKey, TValue> _poolCleaner;

        /// <summary>
        /// Inizializza una nuova istanza della classe PoolManager.
        /// </summary>
        /// <param name="instanceCreator">Una funzione che crea una nuova istanza dell'oggetto.</param>
        /// <param name="maxInstancesPerKey">Opzionale. Il numero massimo di istanze da mantenere nel pool per chiave.</param>
        /// <param name="cleanupInterval">Opzionale. L'intervallo di tempo tra le pulizie del pool.</param>
        /// <param name="maxIdleTime">Opzionale. Il tempo massimo che un oggetto può rimanere inattivo prima di essere rimosso.</param>
        public PoolManager(Func<TKey, TValue> instanceCreator, int? maxInstancesPerKey = null, TimeSpan? cleanupInterval = null, TimeSpan? maxIdleTime = null)
        {
            _instanceCreator = instanceCreator ?? throw new ArgumentNullException(nameof(instanceCreator));
            _maxInstancesPerKey = maxInstancesPerKey;

            // Crea e avvia il cleaner per il pool
            _poolCleaner = new PoolCleaner<TKey, TValue>(_pool, cleanupInterval, maxIdleTime);
            _poolCleaner.StartCleaning();
        }

        /// <summary>
        /// Recupera o crea un'istanza dell'oggetto per la chiave specificata.
        /// </summary>
        /// <param name="key">La chiave per cui recuperare o creare l'oggetto.</param>
        /// <returns>Un'istanza dell'oggetto per la chiave specificata.</returns>
        public TValue GetOrCreate(TKey key)
        {
            if (!_pool.TryGetValue(key, out var instances) || instances.IsEmpty)
            {
                // Crea una nuova istanza se il pool è vuoto o non esiste
                return CreateNewInstance(key);
            }

            // Recupera un'istanza dal pool se disponibile
            if (instances.TryDequeue(out var pooledObj))
            {
                pooledObj.UpdateLastUsedTime(); // Aggiorna l'orario dell'ultimo utilizzo
                return pooledObj.Instance;
            }

            // Crea una nuova istanza se non sono state trovate istanze disponibili nel pool
            return CreateNewInstance(key);
        }

        /// <summary>
        /// Rilascia un'istanza dell'oggetto nel pool.
        /// </summary>
        /// <param name="key">La chiave associata all'istanza dell'oggetto.</param>
        /// <param name="instance">L'istanza dell'oggetto da rilasciare.</param>
        public void Release(TKey key, TValue instance)
        {
            if (!_pool.TryGetValue(key, out var instances))
            {
                // Crea un nuovo pool se non esiste
                instances = new ConcurrentQueue<PooledObject<TValue>>();
                _pool.TryAdd(key, instances);
            }

            // Aggiunge l'istanza al pool se non supera il limite massimo
            if (!_maxInstancesPerKey.HasValue || instances.Count < _maxInstancesPerKey.Value)
            {
                instances.Enqueue(new PooledObject<TValue>(instance));
            }
        }

        /// <summary>
        /// Avvia manualmente il processo di pulizia del pool.
        /// </summary>
        public void StartCleaner()
        {
            _poolCleaner.StartCleaning();
        }

        /// <summary>
        /// Ferma manualmente il processo di pulizia del pool.
        /// </summary>
        public void StopCleaner()
        {
            _poolCleaner.StopCleaning();
        }

        // Metodo privato per creare una nuova istanza e aggiungerla al pool
        private TValue CreateNewInstance(TKey key)
        {
            var newInstance = _instanceCreator(key);
            var pooledObject = new PooledObject<TValue>(newInstance);
            pooledObject.UpdateLastUsedTime();
            return pooledObject.Instance;
        }

        // Implementa anche la gestione del ciclo di vita del PoolManager, ad esempio, IDisposable per fermare il cleaner e rilasciare risorse.
    }
}
