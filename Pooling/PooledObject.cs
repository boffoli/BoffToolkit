namespace BoffToolkit.Pooling
{
    /// <summary>
    /// Rappresenta un oggetto memorizzato nel pool con informazioni sull'ultimo utilizzo.
    /// </summary>
    /// <typeparam name="TValue">Il tipo dell'oggetto memorizzato.</typeparam>
    internal class PooledObject<TValue> where TValue : class
    {
        /// <summary>
        /// Ottiene l'istanza effettiva dell'oggetto.
        /// </summary>
        public TValue Instance { get; }

        /// <summary>
        /// Ottiene o imposta il timestamp dell'ultimo utilizzo.
        /// </summary>
        public DateTime LastUsedTime { get; set; }

        /// <summary>
        /// Inizializza una nuova istanza della classe PooledObject.
        /// </summary>
        /// <param name="instance">L'istanza effettiva dell'oggetto.</param>
        public PooledObject(TValue instance)
        {
            // Assegna l'istanza e imposta l'orario dell'ultimo utilizzo al momento corrente
            Instance = instance ?? throw new ArgumentNullException(nameof(instance), "L'istanza non può essere null.");
            LastUsedTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Aggiorna l'orario dell'ultimo utilizzo all'orario corrente.
        /// </summary>
        public void UpdateLastUsedTime()
        {
            // Aggiorna il timestamp dell'ultimo utilizzo
            LastUsedTime = DateTime.UtcNow;
        }
    }
}
