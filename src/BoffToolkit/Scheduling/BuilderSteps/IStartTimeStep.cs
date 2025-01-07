namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interface for setting the start time of the job scheduler.
    /// </summary>
    public interface IStartTimeStep {
        /// <summary>
        /// Sets the start time of the job scheduler.
        /// </summary>
        /// <param name="startTime">The start time to set.</param>
        /// <returns>An instance of <see cref="IPeriodStep"/> to proceed with the configuration.</returns>
        IPeriodStep SetStart(DateTime startTime);
    }
}