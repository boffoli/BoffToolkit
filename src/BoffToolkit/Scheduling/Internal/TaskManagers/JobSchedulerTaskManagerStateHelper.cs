namespace BoffToolkit.Scheduling.Internal.TaskManagers
{
    /// <summary>
    /// Helper class to manage the state of the JobSchedulerTaskManager in a thread-safe manner.
    /// </summary>
    internal class JobSchedulerTaskManagerStateHelper
    {
        private bool _isRunning;
        private bool _isPaused;
        private readonly object _lock = new();

        /// <summary>
        /// Attempts to set the scheduler as running.
        /// </summary>
        /// <returns><c>true</c> if successfully set to running; otherwise, <c>false</c>.</returns>
        public bool TrySetRunning()
        {
            lock (_lock)
            {
                if (_isRunning)
                    return false;

                _isRunning = true;
                _isPaused = false;
                return true;
            }
        }

        /// <summary>
        /// Sets the scheduler as stopped.
        /// </summary>
        public void SetStopped()
        {
            lock (_lock)
            {
                _isRunning = false;
                _isPaused = false;
            }
        }

        /// <summary>
        /// Attempts to pause the scheduler.
        /// </summary>
        /// <returns><c>true</c> if successfully paused; otherwise, <c>false</c>.</returns>
        public bool TryPause()
        {
            lock (_lock)
            {
                if (!_isRunning || _isPaused)
                    return false;

                _isPaused = true;
                return true;
            }
        }

        /// <summary>
        /// Attempts to resume the scheduler.
        /// </summary>
        /// <returns><c>true</c> if successfully resumed; otherwise, <c>false</c>.</returns>
        public bool TryResume()
        {
            lock (_lock)
            {
                if (!_isRunning || !_isPaused)
                    return false;

                _isPaused = false;
                return true;
            }
        }

        /// <summary>
        /// Determines whether the scheduler is currently running.
        /// </summary>
        /// <returns><c>true</c> if running; otherwise, <c>false</c>.</returns>
        public bool IsRunning()
        {
            lock (_lock)
            {
                return _isRunning;
            }
        }

        /// <summary>
        /// Determines whether the scheduler is currently paused.
        /// </summary>
        /// <returns><c>true</c> if paused; otherwise, <c>false</c>.</returns>
        public bool IsPaused()
        {
            lock (_lock)
            {
                return _isPaused;
            }
        }
    }
}