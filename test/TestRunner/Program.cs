using BoffToolkit.Test;

Console.WriteLine("Esecuzione dei test...");

var jobSchedulerTests = new JobSchedulerTests();

// Chiamata asincrona al test
await jobSchedulerTests.JobScheduler_Should_Invoke_DateTimeAsync();

// Altri test possono essere eseguiti qui
//jobSchedulerTests.JobScheduler_Should_Invoke_HttpGetCall();
/*jobSchedulerTests.JobScheduler_Should_Invoke_Action_Callback();
jobSchedulerTests.JobScheduler_Should_Invoke_ActionWithParam_Callback();
jobSchedulerTests.JobScheduler_Should_Invoke_Func_Callback();
jobSchedulerTests.JobScheduler_Should_Invoke_FuncWithParam_Callback();
jobSchedulerTests.JobScheduler_Should_Invoke_TaskFunc_CallbackAsync().Wait();
jobSchedulerTests.JobScheduler_Should_Invoke_TaskFuncWithParam_CallbackAsync().Wait();
jobSchedulerTests.JobScheduler_Should_Invoke_ISchedulable_Callback();
*/
Console.WriteLine("Test completati.");
