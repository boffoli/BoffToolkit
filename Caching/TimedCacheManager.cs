using System;

namespace BoffToolkit.Caching {
    /// <summary>
    /// Gestisce una cache di oggetti di tipo TValue indicizzata da chiavi di tipo TKey, con scadenza temporizzata.
    /// </summary>
    /// <typeparam name="TKey">Il tipo delle chiavi nella cache.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nella cache.</typeparam>
    public class TimedCacheManager<TKey, TValue> : CacheManager<TKey, TValue> where TKey : notnull {
        private readonly Timer _timer;
        private readonly TimeSpan _timerInterval;
        private readonly Action _timerAction;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="TimedCacheManager{TKey, TValue}"/> con un intervallo di tempo per il controllo periodico della cache e un'azione da eseguire al timer scaduto.
        /// </summary>
        /// <param name="timerInterval">L'intervallo di tempo tra i controlli della cache.</param>
        /// <param name="timerAction">L'azione da eseguire quando scade il timer.</param>
        /// <exception cref="ArgumentException">Viene generata quando l'intervallo del timer è inferiore o uguale a zero.</exception>
        /// <exception cref="ArgumentNullException">Viene generata quando l'azione del timer è nulla.</exception>
        public TimedCacheManager(TimeSpan timerInterval, Action timerAction) : base() {
            // Accetto solo intervalli di tempo superiori a 0
            if (timerInterval <= TimeSpan.Zero)
                throw new ArgumentException("L'intervallo del timer deve essere maggiore di zero.", nameof(timerInterval));
            _timerInterval = timerInterval;

            // Assegno l'azione da fare al timer scaduto
            _timerAction = timerAction ?? throw new ArgumentNullException(nameof(timerAction), "È necessario fornire un'azione valida.");

            // Inizializzo il timer
            _timer = new Timer(_ => _timerAction(), null, _timerInterval, _timerInterval);
        }

        /// <summary>
        /// Rilascia le risorse utilizzate dalla cache temporizzata.
        /// </summary>
        public void Dispose() {
            _timer?.Dispose();
        }
    }
}
