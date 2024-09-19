using BoffToolkit.Scheduling.Internal.States;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Rappresenta il contesto per la gestione degli stati del JobScheduler.
    /// </summary>
    internal class StateContext {
        private IJobSchedulerState _currentState;

        // Costanti per i messaggi di errore
        private const string InitialStateNullErrorMessage = "Lo stato iniziale non può essere null.";
        private const string NewStateNullErrorMessage = "Il nuovo stato non può essere null.";
        private const string CurrentStateNullErrorMessage = "Lo stato corrente non può essere null.";

        /// <summary>
        /// Ottiene il nome dello stato corrente del task scheduler.
        /// </summary>
        /// <value>
        /// Il nome dello stato attualmente attivo.
        /// </value>
        public string StateName => _currentState.Name;

        /// <summary>
        /// Inizializza una nuova istanza della classe <see cref="StateContext"/>.
        /// </summary>
        /// <param name="initialState">Lo stato iniziale del JobScheduler.</param>
        public StateContext(IJobSchedulerState initialState) {
            _currentState = initialState ?? throw new ArgumentNullException(nameof(initialState), InitialStateNullErrorMessage);
            SetState(initialState);
        }

        /// <summary>
        /// Imposta un nuovo stato per il JobScheduler.
        /// </summary>
        /// <param name="newState">Il nuovo stato da impostare.</param>
        public void SetState(IJobSchedulerState newState) {
            _currentState = newState ?? throw new ArgumentNullException(nameof(newState), NewStateNullErrorMessage);
            _currentState.ApplyState();
        }

        /// <summary>
        /// Avvia il JobScheduler.
        /// </summary>
        public void Start() {
            if (_currentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            _currentState.Start(this);
        }

        /// <summary>
        /// Ferma il JobScheduler.
        /// </summary>
        public void Stop() {
            if (_currentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            _currentState.Stop(this);
        }

        /// <summary>
        /// Mette in pausa il JobScheduler.
        /// </summary>
        public void Pause() {
            if (_currentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            _currentState.Pause(this);
        }

        /// <summary>
        /// Riprende il JobScheduler se è stato messo in pausa.
        /// </summary>
        public void Resume() {
            if (_currentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            _currentState.Resume(this);
        }
    }
}