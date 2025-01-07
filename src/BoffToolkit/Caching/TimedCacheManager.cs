namespace BoffToolkit.Caching {
    /// <summary>
    /// Manages a cache of objects of type <typeparamref name="TValue"/> indexed by keys of type <typeparamref name="TKey"/> with timed expiration.
    /// </summary>
    /// <typeparam name="TKey">The type of keys used in the cache.</typeparam>
    /// <typeparam name="TValue">The type of objects stored in the cache.</typeparam>
    public class TimedCacheManager<TKey, TValue> : CacheManager<TKey, TValue>, IDisposable where TKey : notnull {
        private readonly Timer _timer;
        private readonly Action _timerAction;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimedCacheManager{TKey, TValue}"/> class with a time interval for periodic cache checks and an action to perform when the timer expires.
        /// </summary>
        /// <param name="timerInterval">The interval between cache checks.</param>
        /// <param name="timerAction">The action to perform when the timer expires.</param>
        /// <exception cref="ArgumentException">Thrown when the timer interval is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the timer action is null.</exception>
        public TimedCacheManager(TimeSpan timerInterval, Action timerAction) : base() {
            if (timerInterval <= TimeSpan.Zero) {
                throw new ArgumentException("The timer interval must be greater than zero.", nameof(timerInterval));
            }

            _timerAction = timerAction ?? throw new ArgumentNullException(nameof(timerAction), "A valid action must be provided.");
            _timer = new Timer(_ => _timerAction(), null, timerInterval, timerInterval);
        }

        /// <summary>
        /// Releases the resources used by the timed cache manager.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources used by the <see cref="TimedCacheManager{TKey, TValue}"/> instance.
        /// </summary>
        /// <param name="disposing">
        /// A boolean value indicating whether the method is called directly by user code (<c>true</c>) 
        /// or by the runtime from a finalizer (<c>false</c>).
        /// </param>
        /// <remarks>
        /// When <paramref name="disposing"/> is <c>true</c>, this method disposes of the managed resources,
        /// such as the internal <see cref="Timer"/>. Unmanaged resources can also be released here if necessary.
        /// </remarks>
        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    // Dispose managed resources
                    _timer?.Dispose();
                }

                // Mark the instance as disposed
                _disposed = true;
            }
        }
    }
}