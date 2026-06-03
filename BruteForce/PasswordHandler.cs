using System;
using System.Security.Cryptography;
using System.Text;

namespace BruteForce
{
    public class PasswordHandler
    {
        // Requirement 4a: Constant static salt defined in the application
        private const string StaticSalt = "IbrahimSalting_2008!";

        // Combines the raw password with the static salt and returns the SHA256 hash as a hex string
        public string ComputeHash(string rawPassword)
        {
            // TODO: 
            // 1. Concatenate rawPassword + StaticSalt
            // 2. Convert to byte array using Encoding.UTF8.GetBytes()
            // 3. Use SHA256.Create().ComputeHash()
            // 4. Convert the resulting byte array into a hexadecimal string and return it.

            throw new NotImplementedException("Implement the hashing logic here.");
        }

        // Validates if a brute-force guess matches the target hash.
        public bool ValidateGuess(string guess, string targetHash)
        {
            // TODO:
            // 1. Pass the 'guess' into your ComputeHash method above.
            // 2. Compare the result with the 'targetHash'.
            // 3. Return true if they match, false otherwise.

            throw new NotImplementedException("Implement the validation logic here.");
        }

        // Generates a random target password between 4 (inclusive) and 6 (exclusive) characters.
        // Requirement 4b.
        public string GenerateTargetPassword()
        {
            // TODO:
            // 1. Create a Random instance.
            // 2. Pick a length between 4 and 5 (since 6 is exclusive: [4-6) ).
            // 3. Randomly select characters from a defined character set (e.g., a-z) to build the string.

            throw new NotImplementedException("Implement the random password generator here.");
        }
    }
}