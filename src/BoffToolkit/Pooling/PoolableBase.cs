namespace BoffToolkit.Pooling {

    /// <summary>
    /// Classe base astratta che fornisce un comportamento di base per le classi che implementano <see cref="IPoolable"/>.
    /// Utilizza un helper di pool per gestire le operazioni di attivazione, validazione, disattivazione e disposizione.
    /// </summary>
    /// <typeparam name="TPoolableType">Il tipo dell'oggetto gestito nel pool, che implementa <see cref="IPoolable"/>.</typeparam>
    public abstract class PoolableBase<TPoolableType> : IPoolable where TPoolableType : class, IPoolable {
        /// <summary>
        /// L'helper di pool utilizzato per gestire le operazioni di pooling.
        /// </summary>
        protected readonly PoolHelperBase<TPoolableType> _poolHelper;

        /// <summary>
        /// Ottiene o imposta il timestamp dell'ultimo utilizzo dell'oggetto.
        /// </summary>
        public DateTime LastUsedTime { get; set; }

        /// <summary>
        /// Indica se l'oggetto è attivo o no. True se attivo, False altrimenti.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="PoolableBase{TPoolableType}"/>.
        /// </summary>
        protected PoolableBase() {
            _poolHelper = CreatePoolHelper();
        }

        /// <summary>
        /// Crea un'istanza di <see cref="PoolHelperBase{TPoolableType}"/> specifica per la classe derivata.
        /// </summary>
        /// <returns>Un'istanza di <see cref="PoolHelperBase{TPoolableType}"/>.</returns>
        protected abstract PoolHelperBase<TPoolableType> CreatePoolHelper();

        /// <summary>
        /// Attiva l'oggetto con i parametri specificati.
        /// </summary>
        /// <param name="activationParams">Parametri opzionali per l'attivazione.</param>
        public Task ActivateAsync(params object[] activationParams) => _poolHelper.ActivateAsync(activationParams);

        /// <summary>
        /// Valida lo stato corrente dell'oggetto.
        /// </summary>
        /// <returns>True se la validazione ha successo, altrimenti False.</returns>
        public Task<bool> ValidateAsync() => _poolHelper.ValidateAsync();

        /// <summary>
        /// Deattiva l'oggetto.
        /// </summary>
        public Task DeactivateAsync() => _poolHelper.DeactivateAsync();

        /// <summary>
        /// Dispone delle risorse utilizzate dall'oggetto.
        /// </summary>
        public async ValueTask DisposeAsync() {
            await _poolHelper.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}