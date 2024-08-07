using BoffToolkit.Scheduling.Internal.States;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Rappresenta il contesto per la gestione degli stati del JobScheduler.
    /// </summary>
    internal class StateContext {
        private IJobSchedulerState _currentState;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="StateContext"/>.
        /// </summary>
        /// <param name="initialState">Lo stato iniziale del JobScheduler.</param>
        public StateContext(IJobSchedulerState initialState) {
            if (initialState == null)
                throw new ArgumentNullException(nameof(initialState), "Lo stato iniziale non può essere null.");
            
            _currentState = initialState;
            SetState(initialState);
        }

        /// <summary>
        /// Imposta un nuovo stato per il JobScheduler.
        /// </summary>
        /// <param name="newState">Il nuovo stato da impostare.</param>
        public void SetState(IJobSchedulerState newState) {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState), "Il nuovo stato non può essere null.");
            
            _currentState = newState;
            _currentState.ApplyState();
        }

        /// <summary>
        /// Avvia il JobScheduler.
        /// </summary>
        public void Start() {
            if (_currentState == null)
                throw new InvalidOperationException("Lo stato corrente non può essere null.");
            
            _currentState.Start(this);
        }

        /// <summary>
        /// Ferma il JobScheduler.
        /// </summary>
        public void Stop() {
            if (_currentState == null)
                throw new InvalidOperationException("Lo stato corrente non può essere null.");
            
            _currentState.Stop(this);
        }

        /// <summary>
        /// Mette in pausa il JobScheduler.
        /// </summary>
        public void Pause() {
            if (_currentState == null)
                throw new InvalidOperationException("Lo stato corrente non può essere null.");
            
            _currentState.Pause(this);
        }

        /// <summary>
        /// Riprende il JobScheduler se è stato messo in pausa.
        /// </summary>
        public void Resume() {
            if (_currentState == null)
                throw new InvalidOperationException("Lo stato corrente non può essere null.");
            
            _currentState.Resume(this);
        }
    }
}