using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
            // Check if _cts is not null and trigger cancellation.
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }

        // Starts the multi-threaded brute force attack.
        public async Task<string> StartAttackAsync(string targetHash, int maxLength, bool isMultiThreaded, IProgress<BruteForceProgress> progress)
        {
            _cts = new CancellationTokenSource();
            Stopwatch stopwatch = Stopwatch.StartNew();
            long totalAttempts = 0;
            string foundPassword = null;

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
                // I use Task.Run so I don't block the UI thread while Parallel.ForEach works
                await Task.Run(() =>
                {
                    Parallel.ForEach(combinations, parallelOptions, (guess) =>
                    {
                        long currentAttemptCount = Interlocked.Increment(ref totalAttempts);

                        if (currentAttemptCount % 100000 == 0)
                        {
                            progress?.Report(new BruteForceProgress
                            {
                                AttemptsMade = currentAttemptCount,
                                CurrentGuess = guess,
                                Elapsed = stopwatch.Elapsed
                            });
                        }

                        if (_passwordHandler.ValidateGuess(guess, targetHash))
                        {
                            // 4a.
                            foundPassword = guess;

                            // 4b.
                            _cts.Cancel();
                        }
                    });

                }, _cts.Token);
            }
            catch (OperationCanceledException)
            {
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