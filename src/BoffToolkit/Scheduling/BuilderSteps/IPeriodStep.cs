using BoffToolkit.Scheduling.PeriodRules;

namespace BoffToolkit.Scheduling.BuilderSteps {
    /// <summary>
    /// Interfaccia per impostare il periodo di esecuzione.
    /// </summary>
    public interface IPeriodStep {
        /// <summary>
        /// Imposta la regola del periodo di esecuzione.
        /// </summary>
        /// <param name="periodRule">La regola del periodo da impostare.</param>
        /// <returns>Un'istanza di <see cref="ICallbackStep"/>.</returns>
        ICallbackStep SetPeriod(IPeriodRule periodRule);
    }
}