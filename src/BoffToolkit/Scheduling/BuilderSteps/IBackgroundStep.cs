namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interfaccia per impostare l'esecuzione in background.
    /// </summary>
    public interface IBackgroundStep {
        /// <summary>
        /// Imposta se il job deve essere eseguito in background.
        /// </summary>
        /// <param name="isBackground">Se true, il job viene eseguito in background.</param>
        /// <returns>Un'istanza di <see cref="IBuildableStep"/>.</returns>
        IBuildableStep RunInBackground(bool isBackground);
    }
}