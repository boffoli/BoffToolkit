namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for the registration step in the JobScheduler build process.
    /// </summary>
    public interface IRegistrationStep {
        /// <summary>
        /// Specifies whether to register the job scheduler in the global registry.
        /// </summary>
        /// <param name="register">If <c>true</c>, the job scheduler will be registered; otherwise, it will not be registered.</param>
        /// <returns>An instance of <see cref="IBuildableStep"/> to continue the configuration process.</returns>
        IBackgroundStep RegisterScheduler(bool register);
    }
}