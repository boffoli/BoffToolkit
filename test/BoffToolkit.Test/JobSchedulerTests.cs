using BoffToolkit.Scheduling;
using BoffToolkit.Scheduling.HttpCalls;
using BoffToolkit.Scheduling.PeriodRules;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BoffToolkit.Test {
    public class JobSchedulerTests {
        [Fact]
        public void JobScheduler_Should_Invoke_Action_Callback() {
            // Arrange
            var wasCallbackInvoked = false;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));


            
            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(() => wasCallbackInvoked = true)
                .RunInBackground(true)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.True(wasCallbackInvoked);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_ActionWithParam_Callback() {
            // Arrange
            var param = "test";
            var receivedParam = string.Empty;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback<string>((p) => receivedParam = p, param)
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Param: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(param, receivedParam);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_Func_Callback() {
            // Arrange
            var result = 0;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(() => result = 42)
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(42, result);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_FuncWithParam_Callback() {
            // Arrange
            var param = 10;
            var result = 0;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback<int, int>((p) => result = p * 2, param)
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(20, result);
        }

        [Fact]
        public async Task JobScheduler_Should_Invoke_TaskFunc_CallbackAsync() {
            // Arrange
            var result = 0;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(async () => {
                    await Task.Delay(500);
                    result = 42;
                    return result;
                })
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Async Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            await WaitForNextOccurrenceAsync(periodRule);
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task JobScheduler_Should_Invoke_TaskFuncWithParam_CallbackAsync() {
            // Arrange
            var param = 10;
            var result = 0;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(async (p) => {
                    await Task.Delay(500);
                    return result = p * 2;
                }, param)
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: Async Callback with Param Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            await WaitForNextOccurrenceAsync(periodRule);
            Assert.Equal(20, result);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_ISchedulable_Callback() {
            // Arrange
            var schedulable = new SchedulableStub();
            var expectedResult = 42;
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(schedulable)
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: ISchedulable Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
            Assert.Equal(expectedResult, schedulable.Result);
        }

[Fact]
        public void JobScheduler_Should_Invoke_HttpGetCall() {
            // Arrange
            var url = "https://send.araneacloud.it/mail/sendemailtest.php";
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreateGetCall<string>(url))
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: GET Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpPostCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts";
            var data = new { title = "foo", body = "bar", userId = 1 };
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreatePostCall<object, object>(url, data))
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: POST Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpPutCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var data = new { id = 1, title = "foo", body = "bar", userId = 1 };
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreatePutCall<object, object>(url, data))
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: PUT Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
        }

        [Fact]
        public void JobScheduler_Should_Invoke_HttpDeleteCall() {
            // Arrange
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            var periodRule = new TimeSpanPeriodRule(TimeSpan.FromSeconds(1));

            var scheduler = JobSchedulerBuilder
                .SetStartTime(DateTime.Now.AddSeconds(1))
                .SetPeriod(periodRule)
                .SetCallback(HttpCallFactory.CreateDeleteCall<object>(url))
                .RunInBackground(false)
                .SetCallbackCompleted((sender, args) => {
                    Console.WriteLine($"{DateTime.Now}: DELETE Callback Completed with Result: " + args.Result);
                })
                .Build();

            // Act
            scheduler.Start();

            // Assert
            WaitForNextOccurrence(periodRule);
        }

        private static void WaitForNextOccurrence(IPeriodRule periodRule) {
            DateTime now = DateTime.Now;
            DateTime nextOccurrence = periodRule.GetNextOccurrence(now);
            TimeSpan delayToNextOccurrence = nextOccurrence - now + TimeSpan.FromSeconds(10); // Aggiunge un buffer di 10 secondi
            Task.Delay(delayToNextOccurrence).Wait();
        }

        private static async Task WaitForNextOccurrenceAsync(IPeriodRule periodRule) {
            DateTime now = DateTime.Now;
            DateTime nextOccurrence = periodRule.GetNextOccurrence(now);
            TimeSpan delayToNextOccurrence = nextOccurrence - now + TimeSpan.FromSeconds(10); // Aggiunge un buffer di 10 secondi
            await Task.Delay(delayToNextOccurrence);
        }

        private class SchedulableStub : ISchedulable<int> {
            public int Result { get; private set; }
            public int Execute() {
                Result = 42;
                return Result;
            }
        }
    }
}