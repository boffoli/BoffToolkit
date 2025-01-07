using System.Collections.Concurrent;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Global registry for managing centralized instances of <see cref="IJobScheduler"/>.
    /// </summary>
    public static class JobSchedulerRegistry {
        /// <summary>
        /// Static dictionary to hold all instances of <see cref="IJobScheduler"/>.
        /// The key is a unique identifier for each job scheduler.
        /// </summary>
        private static readonly ConcurrentDictionary<string, IJobScheduler> _schedulers = new();

        /// <summary>
        /// Adds a job scheduler to the registry.
        /// </summary>
        /// <param name="id">The unique identifier for the job scheduler.</param>
        /// <param name="scheduler">The instance of <see cref="IJobScheduler"/> to add.</param>
        /// <param name="overwrite">If true, overwrites an existing job scheduler with the same identifier. Default: false.</param>
        /// <returns>True if the addition or replacement is successful; otherwise, false.</returns>
        public static bool Add(string id, IJobScheduler scheduler, bool overwrite = false) {
            EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.

            if (overwrite) {
                _schedulers[id] = scheduler; // Overwrites if overwrite is true
                return true;
            }
            return _schedulers.TryAdd(id, scheduler); // Adds only if it doesn't already exist
        }

        /// <summary>
        /// Removes a job scheduler from the registry.
        /// </summary>
        /// <param name="id">The unique identifier of the job scheduler to remove.</param>
        /// <param name="dispose">If true, calls <see cref="IDisposable.Dispose"/> on the removed instance.</param>
        /// <returns><c>true</c> if the job scheduler was successfully removed; otherwise, <c>false</c>.</returns>
        public static bool Remove(string id, bool dispose) {
            if (_schedulers.TryRemove(id, out var scheduler)) {
                EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.

                if (dispose) {
                    scheduler.Dispose();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves a job scheduler from the registry.
        /// </summary>
        /// <param name="id">The unique identifier of the job scheduler.</param>
        /// <returns>The <see cref="IJobScheduler"/> instance, or null if it doesn't exist or has been released.</returns>
        public static IJobScheduler? Get(string id) {
            if (_schedulers.TryGetValue(id, out var scheduler)) {
                try {
                    EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.
                    return scheduler;
                }
                catch (ObjectDisposedException) {
                    _schedulers.TryRemove(id, out _); // Removes from the registry if already released.
                }
            }
            return null;
        }

        /// <summary>
        /// Returns all job scheduler instances currently in the registry.
        /// </summary>
        /// <returns>A collection of all <see cref="IJobScheduler"/> instances that have not been released.</returns>
        public static IReadOnlyCollection<IJobScheduler> GetAll() {
            var activeSchedulers = new List<IJobScheduler>();
            foreach (var scheduler in _schedulers.Values) {
                try {
                    EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.
                    activeSchedulers.Add(scheduler);
                }
                catch (ObjectDisposedException) {
                    // Ignores released schedulers.
                }
            }
            return activeSchedulers;
        }

        /// <summary>
        /// Checks if a job scheduler exists in the registry.
        /// </summary>
        /// <param name="id">The unique identifier of the job scheduler.</param>
        /// <returns>True if it exists and has not been released; otherwise, false.</returns>
        public static bool Contains(string id) {
            if (_schedulers.TryGetValue(id, out var scheduler)) {
                try {
                    EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.
                    return true;
                }
                catch (ObjectDisposedException) {
                    _schedulers.TryRemove(id, out _); // Removes from the registry if already released.
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all released job schedulers from the registry.
        /// </summary>
        /// <returns>The number of job schedulers removed.</returns>
        public static int Clean() {
            var removedCount = 0;

            foreach (var key in _schedulers.Keys) {
                if (_schedulers.TryGetValue(key, out var scheduler)) {
                    try {
                        EnsureNotDisposed(scheduler); // Checks if the job scheduler has been released.
                    }
                    catch (ObjectDisposedException) {
                        if (_schedulers.TryRemove(key, out _)) {
                            removedCount++;
                        }
                    }
                }
            }

            return removedCount;
        }

        /// <summary>
        /// Ensures that a scheduler has not been released.
        /// </summary>
        /// <param name="scheduler">The scheduler to check.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the scheduler has been released.</exception>
        private static void EnsureNotDisposed(IJobScheduler scheduler) {
            if (scheduler.IsDisposed()) {
                throw new ObjectDisposedException(nameof(IJobScheduler), "Cannot perform operations on a disposed job scheduler.");
            }
        }
    }
}