using System.Diagnostics;

namespace BruteForce
{
    // DTO
    public class BruteForceProgress
    {
        public long AttemptsMade { get; set; }
        public string CurrentGuess { get; set; }
        public TimeSpan Elapsed { get; set; }
    }

    public class TaskOrchestrator
    {
        private readonly PasswordHandler _passwordHandler;
        private readonly BruteForceGenerator _generator;
        private CancellationTokenSource _cts;

        public TaskOrchestrator(PasswordHandler passwordHandler, BruteForceGenerator generator)
        {
            _passwordHandler = passwordHandler;
            _generator = generator;
        }

        // Stops the attack immediately. Requirement 6.
        public void StopAttack()
        {

            throw new NotImplementedException("Implement cancellation logic here.");
        }

        // Starts the multi-threaded brute force attack.
        public async Task<string> StartAttackAsync(string targetHash, int maxLength, bool isMultiThreaded, IProgress<BruteForceProgress> progress)
        {
            _cts = new CancellationTokenSource();
            Stopwatch stopwatch = Stopwatch.StartNew();
            long totalAttempts = 0;
            string foundPassword = null;

            // Get combinations from File 2
            var combinations = _generator.GenerateCombinations(maxLength);

            // Requirement 4e: Max of (CPU Cores - 1)
            int maxThreads = isMultiThreaded ? Math.Max(1, Environment.ProcessorCount - 1) : 1;

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxThreads,
                CancellationToken = _cts.Token
            };

            try
            {
                // We use Task.Run so we don't block the UI thread while Parallel.ForEach works
                await Task.Run(() =>
                {

                }, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // This exception is expected when we call _cts.Cancel().
                // It means the threads stopped successfully.
            }

            stopwatch.Stop();

            // Final progress update so the UI has the exact final time and attempt count
            progress?.Report(new BruteForceProgress
            {
                AttemptsMade = totalAttempts,
                CurrentGuess = foundPassword ?? "Not Found",
                Elapsed = stopwatch.Elapsed
            });

            return foundPassword;
        }
    }
}