namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Interfaccia che definisce lo stato di un task scheduler.
    /// </summary>
    internal interface IJobSchedulerState {
        /// <summary>
        /// Nome dello stato corrente.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Metodo chiamato quando uno stato diventa attivo. 
        /// Esegue le operazioni specifiche dello stato, come l'avvio, la pausa o l'arresto di un task.
        /// </summary>
        void ApplyState();

        /// <summary>
        /// Avvia il task scheduler.
        /// </summary>
        /// <param name="context">Il contesto del job scheduler.</param>
        void Start(StateContext context);

        /// <summary>
        /// Mette in pausa il task scheduler.
        /// </summary>
        /// <param name="context">Il contesto del job scheduler.</param>
        void Pause(StateContext context);

        /// <summary>
        /// Riprende l'esecuzione del task scheduler.
        /// </summary>
        /// <param name="context">Il contesto del job scheduler.</param>
        void Resume(StateContext context);

        /// <summary>
        /// Ferma il task scheduler.
        /// </summary>
        /// <param name="context">Il contesto del job scheduler.</param>
        void Stop(StateContext context);
    }
}