namespace BoffToolkit.Pooling {
    /// <summary>
    /// Interface for objects that can be managed within a pool, providing methods to activate, validate,
    /// deactivate, and clean up the object to ensure efficient and safe reuse.
    /// </summary>
    public interface IPoolable : IAsyncDisposable {
        /// <summary>
        /// Gets or sets the timestamp of the last usage of the object. Used to track activity and manage
        /// the removal of inactive objects from the pool.
        /// </summary>
        DateTime LastUsedTime { get; internal set; }

        /// <summary>
        /// Indicates whether the object is active. True if active, otherwise False.
        /// </summary>
        bool IsActive { get; internal set; }

        /// <summary>
        /// Activates the object, preparing it for use. This method may configure the initial state of the object
        /// or restore settings to a suitable state for usage.
        /// </summary>
        /// <param name="activationParams">Optional parameters used to configure the object during activation.</param>
        Task ActivateAsync(params object[] activationParams);

        /// <summary>
        /// Validates whether the object is in a suitable state for usage. This may include checks on network connections,
        /// active sessions, or consistency of internal state.
        /// </summary>
        /// <returns>True if the object is valid and ready for use, otherwise False.</returns>
        Task<bool> ValidateAsync();

        /// <summary>
        /// Deactivates the object, resetting or preparing it to be returned to the pool. Cleans up temporary states,
        /// sensitive data, and revokes subscriptions or observers to prevent unintended side effects during reuse.
        /// </summary>
        Task DeactivateAsync();
    }
}