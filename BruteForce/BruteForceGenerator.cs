using System;
using System.Collections.Generic;

namespace BruteForce
{
    public class BruteForceGenerator
    {
        // Define the characters your brute force attack will use. 
        // For testing, keeping this small (e.g., just "abcdefghijklmnopqrstuvwxyz") 
        // will make the 6-character brute force finish in a reasonable amount of time.
        private readonly string _characterSet;

        public BruteForceGenerator(string characterSet = "abcdefghijklmnopqrstuvwxyz")
        {
            _characterSet = characterSet;
        }

        /// <summary>
        /// Generates all possible combinations starting from length 1 up to maxLength.
        /// Requirement 4c: Must begin searching from length 1.
        /// </summary>
        public IEnumerable<string> GenerateCombinations(int maxLength)
        {
            // TODO:
            // 1. Create an outer loop that iterates 'currentLength' from 1 up to 'maxLength'.
            // 2. For each 'currentLength', generate all possible string permutations using _characterSet.
            // 3. Use 'yield return [generatedString];' to pass each combination back to the caller one by one.

            throw new NotImplementedException("Implement the combination generation logic here.");
        }

        // Optional but recommended: A private helper method
        // You will likely need a recursive function or a base-N counting loop here 
        // to actually build the strings of 'currentLength'.
        private void GenerateRecursive( /* add necessary parameters here */ )
        {
            // TODO: Implement the underlying math to combine characters.
        }
    }
}