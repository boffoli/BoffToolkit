using System;

namespace BoffToolkit.Scheduling.PeriodRules
{
    /// <summary>
    /// Interfaccia che definisce una regola per determinare la prossima occorrenza di un periodo.
    /// </summary>
    public interface IPeriodRule
    {
        /// <summary>
        /// Restituisce la prossima occorrenza del periodo a partire da un momento specifico.
        /// </summary>
        /// <param name="fromTime">Il momento di partenza dal quale calcolare la prossima occorrenza.</param>
        /// <returns>La prossima occorrenza del periodo.</returns>
        DateTime GetNextOccurrence(DateTime fromTime);
    }
}