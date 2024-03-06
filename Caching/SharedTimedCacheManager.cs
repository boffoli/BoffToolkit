using System;

namespace BoffToolkit.Caching
{
    /// <summary>
    /// Gestisce una cache di oggetti di tipo TValue indicizzata da chiavi di tipo TKey, con scadenza temporizzata, implementando il pattern singleton.
    /// </summary>
    /// <typeparam name="TKey">Il tipo delle chiavi nella cache.</typeparam>
    /// <typeparam name="TValue">Il tipo degli oggetti memorizzati nella cache.</typeparam>
    public class SharedTimedCacheManager<TKey, TValue> : TimedCacheManager<TKey, TValue> where TKey : notnull
    {
        private static Lazy<SharedTimedCacheManager<TKey, TValue>>? _lazyInstance;

        /// <summary>
        /// Ottiene l'istanza condivisa di SharedTimedCacheManager.
        /// </summary>
        public static SharedTimedCacheManager<TKey, TValue> Instance =>
            _lazyInstance?.Value ?? throw new InvalidOperationException("L'istanza di SharedTimedCacheManager non è stata configurata.");

        /// <summary>
        /// Costruttore privato per evitare la creazione di istanze esterne.
        /// </summary>
        private SharedTimedCacheManager(TimeSpan timerInterval, Action timerAction)
            : base(timerInterval, timerAction)
        {
        }

        /// <summary>
        /// Configura l'istanza condivisa di SharedTimedCacheManager con l'intervallo del timer e l'azione specificati.
        /// </summary>
        /// <param name="timerInterval">L'intervallo di tempo tra i controlli della cache.</param>
        /// <param name="timerAction">L'azione da eseguire quando scade il timer.</param>
        public static void Configure(TimeSpan timerInterval, Action timerAction)
        {
            _lazyInstance ??= new Lazy<SharedTimedCacheManager<TKey, TValue>>(
                () => new SharedTimedCacheManager<TKey, TValue>(timerInterval, timerAction));
        }
    }
}
