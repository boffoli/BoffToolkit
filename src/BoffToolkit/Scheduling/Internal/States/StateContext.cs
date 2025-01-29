using BoffToolkit.Scheduling.Internal.States;

namespace BoffToolkit.Scheduling.Internal {
    /// <summary>
    /// Represents the context for managing the states of a JobScheduler.
    /// </summary>
    internal class StateContext {
        /// <summary>
        /// Gets the current state of the JobScheduler.
        /// </summary>
        internal IJobSchedulerState CurrentState { get; private set; }

        // Constants for error messages
        private const string InitialStateNullErrorMessage = "The initial state cannot be null.";
        private const string NewStateNullErrorMessage = "The new state cannot be null.";
        private const string CurrentStateNullErrorMessage = "The current state cannot be null.";

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext"/> class.
        /// </summary>
        /// <param name="initialState">The initial state of the JobScheduler.</param>
        /// <exception cref="ArgumentNullException">Thrown when the initial state is null.</exception>
        public StateContext(IJobSchedulerState initialState) {
            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState), InitialStateNullErrorMessage);
            SetState(initialState);
        }

        /// <summary>
        /// Sets a new state for the JobScheduler.
        /// </summary>
        /// <param name="newState">The new state to set.</param>
        /// <exception cref="ArgumentNullException">Thrown when the new state is null.</exception>
        public void SetState(IJobSchedulerState newState) {
            CurrentState = newState ?? throw new ArgumentNullException(nameof(newState), NewStateNullErrorMessage);
            CurrentState.ApplyState();
        }

        /// <summary>
        /// Starts the JobScheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the current state is null.</exception>
        public void Start() {
            if (CurrentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }
            CurrentState.Start(this);
        }

        /// <summary>
        /// Stops the JobScheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the current state is null.</exception>
        public void Stop() {
            if (CurrentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            CurrentState.Stop(this);
        }

        /// <summary>
        /// Pauses the JobScheduler.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the current state is null.</exception>
        public void Pause() {
            if (CurrentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            CurrentState.Pause(this);
        }

        /// <summary>
        /// Resumes the JobScheduler if it has been paused.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the current state is null.</exception>
        public void Resume() {
            if (CurrentState == null) {
                throw new InvalidOperationException(CurrentStateNullErrorMessage);
            }

            CurrentState.Resume(this);
        }
    }
}