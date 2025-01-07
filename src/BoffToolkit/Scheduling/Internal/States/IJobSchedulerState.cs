namespace BoffToolkit.Scheduling.Internal.States {
    /// <summary>
    /// Interface defining the state of a task scheduler.
    /// </summary>
    internal interface IJobSchedulerState {
        /// <summary>
        /// Determines whether the current state is <c>Stopped</c>.
        /// </summary>
        /// <returns><c>true</c> if the task scheduler is stopped; otherwise, <c>false</c>.</returns>
        bool IsStopped();

        /// <summary>
        /// Determines whether the current state is <c>Paused</c>.
        /// </summary>
        /// <returns><c>true</c> if the task scheduler is paused; otherwise, <c>false</c>.</returns>
        bool IsPaused();

        /// <summary>
        /// Determines whether the current state is <c>Running</c>.
        /// </summary>
        /// <returns><c>true</c> if the task scheduler is running; otherwise, <c>false</c>.</returns>
        bool IsRunning();

        /// <summary>
        /// Method called when a state becomes active.
        /// Executes state-specific operations such as starting, pausing, or stopping a task.
        /// </summary>
        void ApplyState();

        /// <summary>
        /// Starts the task scheduler.
        /// </summary>
        /// <param name="context">The context of the job scheduler.</param>
        void Start(StateContext context);

        /// <summary>
        /// Pauses the task scheduler.
        /// </summary>
        /// <param name="context">The context of the job scheduler.</param>
        void Pause(StateContext context);

        /// <summary>
        /// Resumes the execution of the task scheduler.
        /// </summary>
        /// <param name="context">The context of the job scheduler.</param>
        void Resume(StateContext context);

        /// <summary>
        /// Stops the task scheduler.
        /// </summary>
        /// <param name="context">The context of the job scheduler.</param>
        void Stop(StateContext context);
    }
}