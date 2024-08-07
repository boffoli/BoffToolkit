using BoffToolkit.Logging;


namespace BoffToolkit.Pooling {
    /// <summary>
    /// Classe astratta per la gestione delle operazioni di pooling di oggetti <see cref="IPoolable"/>.
    /// </summary>
    /// <typeparam name="TPoolable">Il tipo di oggetto gestito nel pool, che implementa <see cref="IPoolable"/>.</typeparam>
    /// <remarks>
    /// Inizializza una nuova istanza della classe <see cref="PoolHelperBase{TPoolable}"/>.
    /// </remarks>
    /// <param name="poolable">L'oggetto gestito nel pool.</param>
    /// <exception cref="ArgumentNullException">Sollevata se <paramref name="poolable"/> è null.</exception>
    public abstract class PoolHelperBase<TPoolable>(TPoolable poolable) where TPoolable : class, IPoolable {
        protected readonly TPoolable _poolable = poolable ?? throw new ArgumentNullException(nameof(poolable));

        /// <summary>
        /// Attiva l'oggetto se non è già attivo.
        /// </summary>
        /// <param name="activationParams">Parametri opzionali per l'attivazione.</param>
        public async Task ActivateAsync(params object[] activationParams) {
            if (!_poolable.IsActive) {
                await ActivateSpecificAsync(activationParams);
                _poolable.IsActive = true;
                _poolable.LastUsedTime = DateTime.UtcNow;
                CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} riattivato.");
            }
        }

        /// <summary>
        /// Valida lo stato corrente dell'oggetto.
        /// </summary>
        /// <returns>True se la validazione ha successo, altrimenti False.</returns>
        public async Task<bool> ValidateAsync() {
            var result = _poolable.IsActive;
            try {
                if (!await ValidateSpecificAsync()) {
                    CentralLogger<PoolHelperBase<TPoolable>>.LogError($"Validazione di {typeof(TPoolable).Name} fallita.");
                    return false;
                }
            }
            catch (Exception ex) {
                CentralLogger<PoolHelperBase<TPoolable>>.LogError($"Errore nella validazione di {typeof(TPoolable).Name}: {ex.Message}");
                return false;
            }

            CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"Validazione di {typeof(TPoolable).Name} completata con successo.");
            return result;
        }

        /// <summary>
        /// Deattiva l'oggetto se è attivo.
        /// </summary>
        public async Task DeactivateAsync() {
            if (_poolable.IsActive) {
                await DeactivateSpecificAsync();
                _poolable.IsActive = false;
                CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} deattivato.");
            }
        }

        /// <summary>
        /// Dispone delle risorse utilizzate dal gestore della richiesta.
        /// </summary>
        public async ValueTask DisposeAsync() {
            if (_poolable.IsActive) {
                await DeactivateAsync();
            }
            await DisposeSpecificAsync();
            _poolable.IsActive = false;
            CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} disposed.");
        }

        protected abstract Task ActivateSpecificAsync(params object[] activationParams);
        protected abstract Task<bool> ValidateSpecificAsync();
        protected abstract Task DeactivateSpecificAsync();
        protected abstract Task DisposeSpecificAsync();
    }
}