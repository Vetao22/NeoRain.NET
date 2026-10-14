using System;
using System.Threading;
using System.Threading.Tasks;
using NeoRain.NET;

namespace NeoRainDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var engine = new NeoRainEngine();
            using var cts = new CancellationTokenSource();

            // Run Neo rain in an asynchronous background task
            var rainTask = engine.RunAsync(cts.Token);

            // Let the effect run until the user presses any key
            Console.ReadKey(true);

            // Safely cancel and restore terminal state
            cts.Cancel();
            await rainTask;

            Console.WriteLine("NeoRain session terminated. Welcome back to reality.");
        }
    }
}