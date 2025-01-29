using BoffToolkit.Scheduling;
using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.PeriodRules;
using BoffToolkit.Scheduling.Factories;

namespace BoffToolkit.Test {
    public class JobSchedulerTests {

        [Fact]
        public async Task JobScheduler_Should_Invoke_DateTimeAsync() {
            // Arrange
            var wasCallbackInvoked = false;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(3));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now)
                .SetEnd(DateTime.Now + TimeSpan.FromSeconds(30))
                .SetPeriod(periodRule)
                .SetCallback(() => wasCallbackInvoked = true)
                .AddToRegistry("1")
                .SetCallbackCompleted((sender, args) => {
                    PrintSchedulersState(); // Sostituito Console.WriteLine con il metodo desiderato
                })
                .Build();

            // Act
            scheduler.Start();

            // Delay execution to allow callbacks to be invoked
            Console.WriteLine("Scheduler started. Waiting for 1 minutes...");
            await Task.Delay(TimeSpan.FromMinutes(1)); // Aspetta 5 minuti

            Console.WriteLine("1 minute elapsed. Test completed.");

            // Assert (opzionale, se serve verificare lo stato)
            Assert.True(wasCallbackInvoked, "The callback should have been invoked.");
        }


        [Fact]
        public void JobScheduler_Should_Invoke_Action_Callback() {
            // Arrange
            var wasCallbackInvoked = false;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(() => wasCallbackInvoked = true)
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.True(wasCallbackInvoked, "The callback should have been invoked.");
        }

        [Fact]
        public void JobScheduler_Should_Invoke_ActionWithParam_Callback() {
            // Arrange
            var param = "test";
            var receivedParam = string.Empty;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback<string>((p) => receivedParam = p, param)
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Param: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();
            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(param, receivedParam);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_Func_Callback() {
            // Arrange
            var result = 0;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(() => result = 42)
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();
            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(42, result);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_FuncWithParam_Callback() {
            // Arrange
            var param = 10;
            var result = 0;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback<int, int>((p) => result = p * 2, param)
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();
            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(20, result);
        }

        [Fact]
        public async Task JobScheduler_Should_Invoke_TaskFunc_CallbackAsync() {
            // Arrange
            var result = 0;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(async () => {
                    await Task.Delay(500);
                    result = 42;
                    return result;
                })
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Async Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            await WaitForNextOccurrenceAsync(periodRule);
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task JobScheduler_Should_Invoke_TaskFuncWithParam_CallbackAsync() {
            // Arrange
            var param = 10;
            var result = 0;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(async (p) => {
                    await Task.Delay(500);
                    return result = p * 2;
                }, param)
                .AddToRegistry("sched1", true)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Async Callback with Param Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            await WaitForNextOccurrenceAsync(periodRule);
            Assert.Equal(20, result);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_ISchedulable_Callback() {
            // Arrange
            var schedulable = new SchedulableStub();
            var expectedResult = 42;
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(schedulable)
                .AddToRegistry("sched1", true)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: ISchedulable Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(expectedResult, schedulable.Result);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpGetCall() {
            // Arrange
            var url = "https://send.araneacloud.it/mail/sendemailtest.php";
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreateGetCall<string>(url))
                .AddToRegistry("sched1", true)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: GET Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            // Add assertion for expected result if applicable
            Assert.NotNull(scheduler); // Example assertion
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpPostCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts";
            var data = new { title = "foo", body = "bar", userId = 1 };
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreatePostCall<object, object>(url, data))
                .AddToRegistry("sched1", true)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: POST Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            // Add assertion for expected result if applicable
            Assert.NotNull(scheduler); // Example assertion
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpPutCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var data = new { id = 1, title = "foo", body = "bar", userId = 1 };
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreatePutCall<object, object>(url, data))
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: PUT Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            // Add assertion for expected result if applicable
            Assert.NotNull(scheduler); // Example assertion
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpDeleteCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var periodRule = PeriodRuleFactory.CreateTimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder<ITimeSpanPeriodRule>
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetNoEnd()
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreateDeleteCall<object>(url))
                .AddToRegistry("sched1")
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: DELETE Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            PrintSchedulersState();

            // Assert
            WaitForNextOccurrence(periodRule);
            // Add assertion for expected result if applicable
            Assert.NotNull(scheduler); // Example assertion
        }

        private static void WaitForNextOccurrence(ITimeSpanPeriodRule periodRule) {
            var now = DateTime.Now;
            var nextOccurrence = periodRule.GetNextOccurrence(now);
            var delayToNextOccurrence = nextOccurrence - now + TimeSpan.FromSeconds(10); // Aggiunge un buffer di 10 secondi
            Task.Delay(delayToNextOccurrence).Wait();
        }

        private static async Task WaitForNextOccurrenceAsync(ITimeSpanPeriodRule periodRule) {
            var now = DateTime.Now;
            var nextOccurrence = periodRule.GetNextOccurrence(now);
            var delayToNextOccurrence = nextOccurrence - now + TimeSpan.FromSeconds(10); // Aggiunge un buffer di 10 secondi
            await Task.Delay(delayToNextOccurrence);
        }

        private class SchedulableStub : ISchedulable<int> {
            public int Result { get; private set; }
            public int Execute() {
                Result = 42;
                return Result;
            }
        }

        /// <summary>
        /// Prints the state of all schedulers in the registry to the console.
        /// </summary>
        private static void PrintSchedulersState() {
            var schedulers = JobSchedulerRegistry.GetAll();

            foreach (var scheduler in schedulers) {
                string state;

                if (scheduler.IsDisposed()) {
                    state = "Disposed";
                }
                else if (scheduler.IsRunning()) {
                    state = "Running";
                }
                else if (scheduler.IsPaused()) {
                    state = "Paused";
                }
                else if (scheduler.IsStopped()) {
                    state = "Stopped";
                }
                else {
                    state = "Unknown State";
                }

                Console.WriteLine($"Scheduler Type: {scheduler.GetType().Name}, State: {state}");
            }
        }
    }
}