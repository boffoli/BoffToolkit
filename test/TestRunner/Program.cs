using System;
using BoffToolkit.Test;

namespace TestRunner {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Esecuzione dei test...");

            var jobSchedulerTests = new JobSchedulerTests();
            jobSchedulerTests.JobScheduler_Should_Invoke_DateTime();
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
        }
    }
}