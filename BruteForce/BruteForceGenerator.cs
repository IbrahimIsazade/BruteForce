using System;
using System.Collections.Generic;

namespace BruteForce
{
    public class BruteForceGenerator
    {
        // Define the characters your brute force attack will use. 
        private readonly string _characterSet;

        public BruteForceGenerator(string characterSet = "abcdefghijklmnopqrstuvwxyz")
        {
            _characterSet = characterSet;
        }

        // Generates all possible combinations starting from length 1 up to maxLength.
        // Requirement 4c
        public IEnumerable<string> GenerateCombinations(int maxLength)
        {
            for (int currentLength = 1; currentLength <= maxLength; currentLength++)
            {
                // Create a reusable character buffer for the current length
                char[] buffer = new char[currentLength];

                // 2 & 3. Generate permutations and yield them back one by one.
                foreach (string combination in GenerateRecursive(buffer, 0, currentLength))
                {
                    yield return combination;
                }
            }
        }

        private IEnumerable<string> GenerateRecursive(char[] buffer, int currentPosition, int targetLength)
        {
            if (currentPosition == targetLength)
            {
                yield return new string(buffer);
                yield break; // Stop this branch of recursion
            }

            for (int i = 0; i < _characterSet.Length; i++)
            {
                buffer[currentPosition] = _characterSet[i];

                foreach (string combination in GenerateRecursive(buffer, currentPosition + 1, targetLength))
                {
                    yield return combination; // Bubble the generated string up to the caller
                }
            }
        }
    }
}