namespace BoffToolkit.Scheduling.BuilderInterfaces {
    /// <summary>
    /// Interfaccia per impostare l'orario di inizio del job scheduler.
    /// </summary>
    public interface IStartTimeSetter {
        /// <summary>
        /// Imposta l'orario di inizio del job scheduler.
        /// </summary>
        /// <param name="startTime">L'orario di inizio.</param>
        /// <returns>Un'istanza di <see cref="IPeriodSetter"/>.</returns>
        IPeriodSetter SetStartTime(DateTime startTime);
    }
}