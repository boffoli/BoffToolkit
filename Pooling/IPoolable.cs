/// <summary>
/// Interfaccia per oggetti gestibili all'interno di un pool, fornendo metodi per attivare, validare,
/// deattivare e pulire l'oggetto per garantire il riutilizzo efficiente e sicuro.
/// </summary>
/// <typeparam name="TPooledType">Il tipo dell'oggetto stesso.</typeparam>
public interface IPoolable<TPooledType> : IDisposable
{
    /// <summary>
    /// Ottiene o imposta il timestamp dell'ultimo utilizzo dell'oggetto. Usato per tracciare l'attività e gestire la
    /// rimozione degli oggetti inattivi dal pool.
    /// </summary>
    DateTime LastUsedTime { get; }

    /// <summary>
    /// Indica se l'oggetto è attivo o no. True se attivo, False altrimenti.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Attiva l'oggetto, preparandolo per l'uso. Questo metodo può configurare lo stato iniziale dell'oggetto
    /// o ripristinare le impostazioni a uno stato adatto per l'uso.
    /// </summary>
    /// <param name="activationParams">Parametri opzionali utilizzati per configurare l'oggetto durante l'attivazione.</param>
    void Activate(params object[] activationParams);

    /// <summary>
    /// Verifica se l'oggetto è in uno stato adatto per l'uso. Questo può includere controlli su connessioni di rete,
    /// sessioni attive o coerenza di stato interno.
    /// </summary>
    /// <returns>True se l'oggetto è valido e pronto per l'uso, altrimenti False.</returns>
    bool Validate();

    /// <summary>
    /// Deattiva l'oggetto, resettandolo o preparandolo per essere rimesso nel pool. Pulisce stati temporanei,
    /// dati sensibili, e revoca sottoscrizioni o osservatori per prevenire effetti collaterali indesiderati durante
    /// il riutilizzo.
    /// </summary>
    void Deactivate();
    
}
