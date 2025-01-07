namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for configuring background execution of a job.
    /// </summary>
    public interface IBackgroundStep {
        /// <summary>
        /// Configures whether the job should run in the background.
        /// </summary>
        /// <param name="isBackground">If <c>true</c>, the job will execute in the background.</param>
        /// <returns>An instance of <see cref="IBuildableStep"/> to continue the build process.</returns>
        IBuildableStep RunInBackground(bool isBackground);
    }
}