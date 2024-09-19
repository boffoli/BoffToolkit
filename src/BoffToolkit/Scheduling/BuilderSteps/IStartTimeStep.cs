namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interfaccia per impostare l'orario di inizio del job scheduler.
    /// </summary>
    public interface IStartTimeStep {
        /// <summary>
        /// Imposta l'orario di inizio del job scheduler.
        /// </summary>
        /// <param name="startTime">L'orario di inizio.</param>
        /// <returns>Un'istanza di <see cref="IPeriodStep"/>.</returns>
        IPeriodStep SetStart(DateTime startTime);
    }
}