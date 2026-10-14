using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeoRain.NET
{
    /// <summary>
    /// High-performance engine simulating the digital Neo rain effect in the terminal.
    /// </summary>
    public sealed class NeoRainEngine
    {
        private static readonly Random Random = new Random();
        private readonly char[] _charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&*+-<>~|/\\ｦｱｳｴｵｶｷｹｺｻｼｽｾｿﾀﾂﾃﾅﾆﾇﾈﾊﾋﾎﾏﾐﾑﾒﾓﾔﾕﾗﾘﾜ".ToCharArray();

        /// <summary>
        /// Runs the Neo rain animation loop until a key is pressed.
        /// </summary>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.Clear();

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            // Track state for each column on the screen
            int[] drops = new int[width];
            int[] speeds = new int[width];

            for (int i = 0; i < width; i++)
            {
                drops[i] = Random.Next(-height, 0);
                speeds[i] = Random.Next(1, 4); // Random falling speed per column
            }

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Handle dynamic window resizing gracefully
                    if (Console.WindowWidth != width || Console.WindowHeight != height)
                    {
                        width = Console.WindowWidth;
                        height = Console.WindowHeight;
                        drops = new int[width];
                        speeds = new int[width];
                        for (int i = 0; i < width; i++)
                        {
                            drops[i] = Random.Next(-height, 0);
                            speeds[i] = Random.Next(1, 4);
                        }
                        Console.Clear();
                    }

                    for (int x = 0; x < width; x++)
                    {
                        int y = drops[x];

                        // Draw trailing/fading character if within screen bounds
                        if (y >= 0 && y < height)
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(_charset[Random.Next(_charset.Length)]);
                        }

                        // Erase/fade character above the drop trail
                        int trailY = y - 10;
                        if (trailY >= 0 && trailY < height)
                        {
                            Console.SetCursorPosition(x, trailY);
                            Console.Write(' ');
                        }

                        // Advance drop position based on its speed
                        drops[x] += speeds[x];

                        // Reset drop to top once it falls past the screen bottom
                        if (drops[x] >= height)
                        {
                            drops[x] = Random.Next(-10, 0);
                            speeds[x] = Random.Next(1, 4);
                        }
                    }

                    // Frame rate throttling (~33 FPS for smooth cascading effect)
                    await Task.Delay(30, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // Graceful cancellation exit
            }
            finally
            {
                Console.ResetColor();
                Console.CursorVisible = true;
                Console.Clear();
            }
        }
    }
}