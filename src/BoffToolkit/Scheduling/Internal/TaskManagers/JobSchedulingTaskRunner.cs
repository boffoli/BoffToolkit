using BoffToolkit.Logging;
using BoffToolkit.Scheduling.Internal.Contexts;

namespace BoffToolkit.Scheduling.Internal.TaskManagers;

/// <summary>
/// Manages the execution of scheduled tasks, handling timing and callback execution.
/// </summary>
internal static class JobSchedulingTaskRunner {
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static bool _isRunning = false;

    /// <summary>
    /// Executes the scheduling task asynchronously.
    /// </summary>
    /// <param name="context">The scheduler context containing scheduling details.</param>
    /// <param name="token">The cancellation token to monitor for cancellation requests.</param>
    /// <param name="isPaused">A delegate to check if the scheduler is paused.</param>
    public static async Task RunAsync(
        JobSchedulerContext context,
        CancellationToken token,
        Func<bool> isPaused) {

        // Tentativo di acquisire il semaforo senza attesa
        if (!await _semaphore.WaitAsync(0)) {
            CentralLogger<JobSchedulerTaskManager>.LogWarning("RunAsync is already running.");
            return;
        }

        try {
            if (_isRunning) {
                CentralLogger<JobSchedulerTaskManager>.LogWarning("RunAsync is already running.");
                return;
            }

            _isRunning = true;

            while (!token.IsCancellationRequested) {
                var now = DateTime.Now;
                var nextExecution = CalculateNextExecutionTime(context, now);

                // Attende fino alla prossima esecuzione pianificata
                await WaitUntilNextExecutionAsync(nextExecution, token);

                // Se lo scheduler è in pausa, salta l'esecuzione
                if (isPaused()) {
                    continue;
                }

                // Controlla nuovamente la cancellazione prima di eseguire il callback
                if (!token.IsCancellationRequested) {
                    // Avvia il callback in modalità asincrona, senza attenderlo
                    _ = Task.Run(async () => await PerformScheduledTaskAsync(context));
                }
            }
        }
        catch (TaskCanceledException) {
            // Gestione elegante della cancellazione del task
            CentralLogger<JobSchedulerTaskManager>.LogInformation("Scheduling task was canceled.");
        }
        catch (Exception ex) {
            CentralLogger<JobSchedulerTaskManager>.LogError($"Unexpected error: {ex.Message}");
        }
        finally {
            _isRunning = false;
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Attende fino all'orario di esecuzione pianificato, rispettando eventuali cancellazioni.
    /// </summary>
    /// <param name="nextExecution">Il prossimo orario pianificato.</param>
    /// <param name="token">Il token di cancellazione.</param>
    private static async Task WaitUntilNextExecutionAsync(DateTime nextExecution, CancellationToken token) {
        var delay = nextExecution - DateTime.Now;
        if (delay > TimeSpan.Zero) {
            try {
                await Task.Delay(delay, token);
            }
            catch (TaskCanceledException) {
                // Il ritardo è stato interrotto a causa di una richiesta di cancellazione
                CentralLogger<JobSchedulerTaskManager>.LogInformation("Wait was canceled.");
                throw;
            }
        }
    }

    /// <summary>
    /// Esegue il callback pianificato in modo asincrono.
    /// </summary>
    /// <param name="context">Il contesto del job scheduler contenente il callback da eseguire.</param>
    private static async Task PerformScheduledTaskAsync(JobSchedulerContext context) {
        try {
            await context.CallbackAdapter.ExecuteAsync();
            CentralLogger<JobSchedulerTaskManager>.LogInformation("Callback executed successfully.");
        }
        catch (Exception ex) {
            CentralLogger<JobSchedulerTaskManager>.LogError($"Error executing callback: {ex.Message}");
        }
    }

    /// <summary>
    /// Calcola il prossimo orario di esecuzione in base alle regole di schedulazione.
    /// </summary>
    /// <param name="context">Il contesto contenente le regole di schedulazione.</param>
    /// <param name="fromTime">L'orario da cui calcolare la prossima esecuzione.</param>
    /// <returns>Il prossimo orario di esecuzione calcolato.</returns>
    private static DateTime CalculateNextExecutionTime(JobSchedulerContext context, DateTime fromTime) {
        try {
            return context.PeriodRule.GetNextOccurrence(fromTime);
        }
        catch (Exception ex) {
            CentralLogger<JobSchedulerTaskManager>.LogError($"Error calculating next occurrence: {ex.Message}");
            throw;
        }
    }
}