# Multi-Threaded Brute Force Attacker

A C# Windows Forms application designed to demonstrate and compare the performance differences between single-threaded and multi-threaded brute-force decryption attacks. 

This project serves as an educational tool to understand CPU core utilization, thread-safe memory management, lazy evaluation, and the sheer computational power required to crack cryptographic hashes.

## 🚀 Features

* **Multi-Core Utilization:** Toggle between single-threaded execution and multi-threaded execution (utilizing `Environment.ProcessorCount - 1` cores) to see real-time speed differences.
* **Custom & Random Targets:** Enter your own custom string to test, or let the system generate a random 4-5 character target.
* **Secure Hashing:** Simulates real-world database security by concatenating raw passwords with a static salt and hashing them via SHA256.
* **Memory-Optimized Generation:** Uses recursive lazy evaluation (`IEnumerable` and `yield return`) with a recyclable character buffer to prevent `OutOfMemoryExceptions` when generating millions of permutations.
* **Non-Blocking UI:** Employs `async/await` and `IProgress<T>` with throttled reporting to keep the UI completely responsive during heavy background processing.
* **Emergency Stop:** Safely abort active attacks across all threads instantly using `CancellationTokenSource`.
* **Automated Logging:** Writes timestamps, attempts, durations, and mathematical speedup comparisons to a local `performance_log.txt` file.

## 🏗️ Architecture

The application is built using a clean, modular architecture separating the UI from the background logic.

### Components
* **`MainForm`**: The code-first GUI controller handling state and user inputs.
* **`TaskOrchestrator`**: The multi-threading engine utilizing `Parallel.ForEach`.
* **`BruteForceGenerator`**: The recursive string permutation engine.
* **`PasswordHandler`**: Handles standard SHA256 cryptographic hashing and validation.
* **`PerformanceLogger`**: File I/O handler for saving A/B test results.
