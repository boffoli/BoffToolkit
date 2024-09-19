using System;

namespace BoffToolkit.Scheduling {
    /// <summary>
    /// Definisce un'interfaccia per un oggetto schedulabile che può eseguire una determinata azione e restituire un risultato.
    /// </summary>
    /// <typeparam name="TResult">Il tipo del risultato prodotto dall'operazione di schedulazione.</typeparam>
    public interface ISchedulable<out TResult> {
        /// <summary>
        /// Esegue l'azione definita e restituisce il risultato.
        /// </summary>
        /// <returns>Il risultato dell'operazione.</returns>
        TResult Execute();
    }

    /// <summary>
    /// Definisce un'interfaccia per un oggetto schedulabile che può eseguire una determinata azione con un parametro e restituire un risultato.
    /// </summary>
    /// <typeparam name="TParam">Il tipo del parametro passato all'operazione di schedulazione.</typeparam>
    /// <typeparam name="TResult">Il tipo del risultato prodotto dall'operazione di schedulazione.</typeparam>
    public interface ISchedulable<in TParam, out TResult> {
        /// <summary>
        /// Esegue l'azione definita utilizzando il parametro fornito e restituisce il risultato.
        /// </summary>
        /// <param name="param">Il parametro passato all'operazione di schedulazione.</param>
        /// <returns>Il risultato dell'operazione.</returns>
        TResult Execute(TParam param);
    }
}