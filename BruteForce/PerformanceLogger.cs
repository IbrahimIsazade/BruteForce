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

        }

        // Logs a direct comparison between the two methods after both have been run.
        public void LogComparison(TimeSpan singleThreadDuration, TimeSpan multiThreadDuration)
        {

        }
    }
}