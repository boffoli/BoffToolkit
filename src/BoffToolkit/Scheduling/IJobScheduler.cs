namespace BoffToolkit.Scheduling {
    public interface IJobScheduler {
        /// <summary>
        /// Avvia l'esecuzione dei task schedulati.
        /// </summary>
        void Start();

        /// <summary>
        /// Ferma l'esecuzione dei task schedulati.
        /// </summary>
        void Stop();

        /// <summary>
        /// Mette in pausa l'esecuzione dei task schedulati.
        /// </summary>
        void Pause();

        /// <summary>
        /// Riprende l'esecuzione dei task schedulati.
        /// </summary>
        void Resume();

        /// <summary>
        /// Restituisce il nome dello stato corrente.
        /// </summary>
        string CurrentStateName { get; }
    }
}