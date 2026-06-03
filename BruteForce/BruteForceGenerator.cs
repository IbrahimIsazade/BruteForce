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
        public void GenerateCombinations(int maxLength)
        {
            
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
}