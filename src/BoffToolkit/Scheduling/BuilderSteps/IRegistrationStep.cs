namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interfaccia per il passo di registrazione nel processo di costruzione del JobScheduler.
    /// </summary>
    public interface IRegistrationStep {
        /// <summary>
        /// Indica se registrare il job scheduler nel registro globale.
        /// </summary>
        /// <param name="register">Se true, il job scheduler verrà registrato; se false, non verrà registrato.</param>
        /// <returns>Un'istanza di <see cref="IBuildableStep"/> per continuare la configurazione.</returns>
        IBackgroundStep RegisterScheduler(bool register);
    }
}