using System.Collections.Concurrent;
using BoffToolkit.Logging;

namespace BoffToolkit.Pooling {
    /// <summary>
    /// Gestisce un pool di oggetti per ogni chiave, riutilizzando gli oggetti anziché crearne di nuovi ogni volta.
    /// Questo aiuta a migliorare le prestazioni e a ridurre il sovraccarico della creazione di oggetti.
    /// </summary>
    /// <typeparam name="TKey">Il tipo della chiave utilizzata per identificare ogni pool.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nei pool, che deve implementare IPoolable&lt;TValue&gt;.</typeparam>
    public class PoolManager<TKey, TValue>
        where TKey : notnull
        where TValue : class, IPoolable<TValue> {
        private readonly ConcurrentDictionary<TKey, ConcurrentQueue<TValue>> _pool
                             = new ConcurrentDictionary<TKey, ConcurrentQueue<TValue>>();
        private readonly Func<TKey, TValue> _instanceCreator;
        private readonly int? _maxInstancesPerKey;
        private readonly PoolCleaner<TKey, TValue> _poolCleaner;

        /// <summary>
        /// Inizializza una nuova istanza della classe PoolManager con i parametri specificati.
        /// </summary>
        /// <param name="instanceCreator">Una funzione che crea una nuova istanza dell'oggetto.</param>
        /// <param name="maxInstancesPerKey">Il numero massimo di istanze da mantenere nel pool per chiave.</param>
        /// <param name="cleanupInterval">L'intervallo di tempo tra le pulizie periodiche.</param>
        /// <param name="maxIdleTime">Il tempo massimo di inattività prima che un oggetto venga rimosso.</param>
        /// <exception cref="ArgumentNullException">Sollevata se <paramref name="instanceCreator"/>, <paramref name="cleanupInterval"/> o <paramref name="maxIdleTime"/> è null o zero.</exception>
        public PoolManager(Func<TKey, TValue> instanceCreator, int maxInstancesPerKey, TimeSpan cleanupInterval, TimeSpan maxIdleTime) {
            _instanceCreator = instanceCreator ?? throw new ArgumentNullException(nameof(instanceCreator));
            _maxInstancesPerKey = maxInstancesPerKey;

            // Crea e avvia il cleaner per il pool
            _poolCleaner = new PoolCleaner<TKey, TValue>(_pool, cleanupInterval, maxIdleTime);
            _poolCleaner.StartCleaning();
        }

        /// <summary>
        /// Recupera un'istanza dell'oggetto per la chiave specificata o ne crea una nuova se non è disponibile.
        /// </summary>
        /// <param name="key">La chiave per cui recuperare o creare l'oggetto.</param>
        /// <param name="activationParams">Parametri opzionali utilizzati per configurare l'oggetto durante l'attivazione.</param>
        /// <returns>Un'istanza dell'oggetto per la chiave specificata, attivata e pronta all'uso.</returns>
        public async Task<TValue> GetOrCreateAsync(TKey key, params object[] activationParams) {
            if (!_pool.TryGetValue(key, out var instances) || instances.IsEmpty) {
                CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Nessuna istanza disponibile nel pool, creazione di una nuova istanza per la chiave {key}.");
                return await CreateNewInstanceAsync(key, activationParams);
            }

            while (instances.TryDequeue(out var instance)) {
                await instance.ActivateAsync(activationParams);
                CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Istanza per la chiave {key}, è stata attivata.");

                if (await instance.ValidateAsync()) {
                    CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Istanza già attivata per la chiave {key}, è stata validata.");
                    return instance;
                }
                else {
                    CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Fallimento della validazione dell'istanza per la chiave {key}, inizio pulizia.");
                    await instance.DisposeAsync();
                }
            }

            CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Creazione di una nuova istanza per la chiave {key} dopo fallimenti di validazione.");
            return await CreateNewInstanceAsync(key, activationParams);
        }

        /// <summary>
        /// Rilascia un'istanza dell'oggetto nel pool, preparandola per essere riutilizzata.
        /// </summary>
        /// <param name="key">La chiave associata all'istanza dell'oggetto.</param>
        /// <param name="instance">L'istanza dell'oggetto da rilasciare.</param>
        public async void ReleaseAsync(TKey key, TValue instance) {
            await instance.DeactivateAsync();
            CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Istanza disattivata per la chiave {key}.");

            var instances = _pool.GetOrAdd(key, _ => new ConcurrentQueue<TValue>());
            if (instances.Count < _maxInstancesPerKey) {
                instances.Enqueue(instance);
                CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Istanza rimessa nel pool per la chiave {key}.");
            }
            else {
                CentralLogger<PoolManager<TKey, TValue>>.LogInformation($"Il pool per la chiave {key} è pieno, inizio pulizia dell'istanza.");
                await instance.DisposeAsync();
            }
        }

        /// <summary>
        /// Crea una nuova istanza dell'oggetto e la attiva utilizzando la funzione di creazione fornita.
        /// </summary>
        /// <param name="key">La chiave per cui creare l'oggetto.</param>
        /// <param name="activationParams">Parametri opzionali utilizzati per configurare l'oggetto durante l'attivazione.</param>
        /// <returns>Una nuova istanza dell'oggetto, già attivata.</returns>
        private async Task<TValue> CreateNewInstanceAsync(TKey key, params object[] activationParams) {
            var newInstance = _instanceCreator(key);
            await newInstance.ActivateAsync(activationParams);
            return newInstance;
        }
    }
}
