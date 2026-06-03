using System;
using System.IO;

namespace BruteForce
{
    public class PerformanceLogger
    {
        private readonly string _logFilePath;

        public PerformanceLogger(string logFilePath = "performance_log.txt")
        {
            _logFilePath = logFilePath;
        }

        // Logs the result of a single attack run (Requirement 8)
        public void LogRun(string attackType, string foundPassword, TimeSpan duration, long totalAttempts)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string formattedString = $"[{timestamp}] Type: {attackType,-15} | " +
                                     $"Result: {foundPassword ?? "Not Found",-8} | " +
                                     $"Time: {duration.TotalSeconds:F3}s | " +
                                     $"Attempts: {totalAttempts}";

            File.AppendAllText(_logFilePath, formattedString + Environment.NewLine);
        }

        // Logs a direct comparison between the two methods after both have been run.
        public void LogComparison(TimeSpan singleThreadDuration, TimeSpan multiThreadDuration)
        {
            double multiMs = Math.Max(multiThreadDuration.TotalMilliseconds, 0.001);
            double speedup = singleThreadDuration.TotalMilliseconds / multiMs;

            // Format a summary string
            string summary = $"--- PERFORMANCE SUMMARY ---{Environment.NewLine}" +
                             $"Single-Threaded Time : {singleThreadDuration.TotalSeconds:F3} seconds{Environment.NewLine}" +
                             $"Multi-Threaded Time  : {multiThreadDuration.TotalSeconds:F3} seconds{Environment.NewLine}" +
                             $"Result               : Multi-threading was {speedup:F2}x faster.{Environment.NewLine}" +
                             $"---------------------------";

            File.AppendAllText(_logFilePath, summary + Environment.NewLine);
        }
    }
}