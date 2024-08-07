using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderInterfaces {
    /// <summary>
    /// Interfaccia per impostare il periodo di esecuzione.
    /// </summary>
    public interface IPeriodSetter {
        /// <summary>
        /// Imposta la regola del periodo di esecuzione.
        /// </summary>
        /// <param name="periodRule">La regola del periodo da impostare.</param>
        /// <returns>Un'istanza di <see cref="ICallbackSetter"/>.</returns>
        ICallbackSetter SetPeriod(IPeriodRule periodRule);
    }
}