using BoffToolkit.Logging;

namespace BoffToolkit.Pooling {
    /// <summary>
    /// Abstract base class for managing pooling operations of <see cref="IPoolable"/> objects.
    /// </summary>
    /// <typeparam name="TPoolable">
    /// The type of object managed in the pool, which implements <see cref="IPoolable"/>.
    /// </typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PoolHelperBase{TPoolable}"/> class.
    /// </remarks>
    /// <param name="poolable">The object managed in the pool.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="poolable"/> is <c>null</c>.
    /// </exception>
    public abstract class PoolHelperBase<TPoolable>(TPoolable poolable) where TPoolable : class, IPoolable {
        /// <summary>
        /// The object managed in the pool.
        /// </summary>
        protected readonly TPoolable _poolable = poolable ?? throw new ArgumentNullException(nameof(poolable));

        /// <summary>
        /// Activates the object if it is not already active.
        /// </summary>
        /// <param name="activationParams">Optional parameters for activation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous activation operation.</returns>
        public async Task ActivateAsync(params object[] activationParams) {
            if (!_poolable.IsActive) {
                await ActivateSpecificAsync(activationParams);
                _poolable.IsActive = true;
                _poolable.LastUsedTime = DateTime.UtcNow;
                CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} activated.");
            }
        }

        /// <summary>
        /// Validates the current state of the object.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the validation succeeds; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ValidateAsync() {
            if (!_poolable.IsActive) {
                return false;
            }

            try {
                if (!await ValidateSpecificAsync()) {
                    CentralLogger<PoolHelperBase<TPoolable>>.LogError($"Validation of {typeof(TPoolable).Name} failed.");
                    return false;
                }
            }
            catch (Exception ex) {
                CentralLogger<PoolHelperBase<TPoolable>>.LogError($"Validation error for {typeof(TPoolable).Name}: {ex.Message}");
                return false;
            }

            CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"Validation of {typeof(TPoolable).Name} succeeded.");
            return true;
        }

        /// <summary>
        /// Deactivates the object if it is active.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous deactivation operation.</returns>
        public async Task DeactivateAsync() {
            if (_poolable.IsActive) {
                await DeactivateSpecificAsync();
                _poolable.IsActive = false;
                CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} deactivated.");
            }
        }

        /// <summary>
        /// Disposes resources used by the object manager.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous disposal operation.</returns>
        public async ValueTask DisposeAsync() {
            if (_poolable.IsActive) {
                await DeactivateAsync();
            }
            await DisposeSpecificAsync();
            _poolable.IsActive = false;
            CentralLogger<PoolHelperBase<TPoolable>>.LogInformation($"{typeof(TPoolable).Name} disposed.");
        }

        /// <summary>
        /// Activates the object in a manner specific to the derived type.
        /// </summary>
        /// <param name="activationParams">Optional parameters for activation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task ActivateSpecificAsync(params object[] activationParams);

        /// <summary>
        /// Validates the object's state in a manner specific to the derived type.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation, 
        /// with a boolean result indicating whether validation succeeded.
        /// </returns>
        protected abstract Task<bool> ValidateSpecificAsync();

        /// <summary>
        /// Deactivates the object in a manner specific to the derived type.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task DeactivateSpecificAsync();

        /// <summary>
        /// Disposes resources in a manner specific to the derived type.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task DisposeSpecificAsync();
    }
}