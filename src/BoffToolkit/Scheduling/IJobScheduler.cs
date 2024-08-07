using System;
using BoffToolkit.Scheduling.Internal.Callbacks;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Interfaccia che definisce le operazioni di base per un job scheduler.
    /// </summary>
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
    }
}