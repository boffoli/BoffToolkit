using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BoffToolkit.Scheduling;

namespace BoffToolkit.Scheduling.Registry {
    /// <summary>
    /// Registro globale dei job scheduler per la gestione centralizzata delle istanze di <see cref="IJobScheduler"/>.
    /// </summary>
    public static class JobSchedulerRegistry {
        /// <summary>
        /// Dizionario statico per contenere tutte le istanze di <see cref="IJobScheduler"/>.
        /// La chiave è un identificatore univoco per ogni job scheduler.
        /// </summary>
        private static readonly ConcurrentDictionary<string, IJobScheduler> _schedulers = new ConcurrentDictionary<string, IJobScheduler>();

        /// <summary>
        /// Aggiunge un job scheduler al registro.
        /// </summary>
        /// <param name="id">L'identificatore univoco per il job scheduler.</param>
        /// <param name="scheduler">L'istanza di <see cref="IJobScheduler"/> da aggiungere.</param>
        /// <param name="overwrite">Se true, sovrascrive un job scheduler esistente con lo stesso identificatore. Valore predefinito: false.</param>
        /// <returns>True se l'aggiunta o la sostituzione avviene con successo, false altrimenti.</returns>
        public static bool Add(string id, IJobScheduler scheduler, bool overwrite = false) {
            if (overwrite) {
                _schedulers[id] = scheduler; // Sovrascrive se overwrite è true
                return true;
            }
            return _schedulers.TryAdd(id, scheduler); // Aggiunge solo se non esiste già
        }

        /// <summary>
        /// Rimuove un job scheduler dal registro.
        /// </summary>
        /// <param name="id">L'identificatore univoco del job scheduler da rimuovere.</param>
        /// <returns>True se il job scheduler è stato rimosso con successo, false altrimenti.</returns>
        public static bool Remove(string id) {
            return _schedulers.TryRemove(id, out _);
        }

        /// <summary>
        /// Ottiene un job scheduler dal registro.
        /// </summary>
        /// <param name="id">L'identificatore univoco del job scheduler.</param>
        /// <returns>L'istanza di <see cref="IJobScheduler"/>, o null se non esiste.</returns>
        public static IJobScheduler? Get(string id) {
            _schedulers.TryGetValue(id, out var scheduler);
            return scheduler;
        }

        /// <summary>
        /// Restituisce tutte le istanze di job scheduler presenti nel registro.
        /// </summary>
        /// <returns>Una collezione di tutte le istanze di <see cref="IJobScheduler"/>.</returns>
        public static IReadOnlyCollection<IJobScheduler> GetAll() {
            return _schedulers.Values.ToList();
        }
        
        /// <summary>
        /// Verifica se un job scheduler esiste nel registro.
        /// </summary>
        /// <param name="id">L'identificatore univoco del job scheduler.</param>
        /// <returns>True se esiste, false altrimenti.</returns>
        public static bool Contains(string id) {
            return _schedulers.ContainsKey(id);
        }
    }
}