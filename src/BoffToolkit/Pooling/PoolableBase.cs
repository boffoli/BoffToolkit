namespace BoffToolkit.Pooling {

    /// <summary>
    /// Abstract base class that provides foundational behavior for classes implementing <see cref="IPoolable"/>.
    /// Utilizes a pool helper to manage activation, validation, deactivation, and disposal operations.
    /// </summary>
    /// <typeparam name="TPoolableType">The type of object managed in the pool, which implements <see cref="IPoolable"/>.</typeparam>
    public abstract class PoolableBase<TPoolableType> : IPoolable where TPoolableType : class, IPoolable {
        /// <summary>
        /// The pool helper used to manage pooling operations.
        /// </summary>
        protected readonly PoolHelperBase<TPoolableType> _poolHelper;

        /// <inheritdoc/>
        public DateTime LastUsedTime { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolableBase{TPoolableType}"/> class.
        /// </summary>
        protected PoolableBase() {
            _poolHelper = CreatePoolHelper();
        }

        /// <summary>
        /// Creates an instance of <see cref="PoolHelperBase{TPoolableType}"/> specific to the derived class.
        /// </summary>
        /// <returns>An instance of <see cref="PoolHelperBase{TPoolableType}"/>.</returns>
        protected abstract PoolHelperBase<TPoolableType> CreatePoolHelper();

        /// <inheritdoc/>
        public Task ActivateAsync(params object[] activationParams) => _poolHelper.ActivateAsync(activationParams);

        /// <inheritdoc/>
        public Task<bool> ValidateAsync() => _poolHelper.ValidateAsync();

        /// <inheritdoc/>
        public Task DeactivateAsync() => _poolHelper.DeactivateAsync();

        /// <inheritdoc/>
        public async ValueTask DisposeAsync() {
            await _poolHelper.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}