using System;
using System.Security.Cryptography;
using System.Text;

namespace BruteForce
{
    public class PasswordHandler
    {
        // Requirement 4a: Constant static salt defined in the application
        private const string StaticSalt = "IbrahimSalting_2008!";

        // Raw password + static salt, then returns the SHA256 hash as hex string
        public string ComputeHash(string rawPassword)
        {
            string saltedPassword = rawPassword + StaticSalt;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(saltedPassword);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the resulting byte array into a hexadecimal string
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // "x2" formats to lowercase hex
                }

                return builder.ToString();
            }
        }

        // Validates if a brute-force guess matches the target hash.
        public bool ValidateGuess(string guess, string targetHash)
        {
            string guessHash = ComputeHash(guess);

            return string.Equals(guessHash, targetHash, StringComparison.OrdinalIgnoreCase);
        }

        // Generates a random target password between 4 (inclusive) and 6 (exclusive) characters.
        // Requirement 4b.
        public string GenerateTargetPassword()
        {
            Random random = new Random();

            int length = random.Next(4, 6);

            const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder passwordBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                passwordBuilder.Append(chars[index]);
            }

            return passwordBuilder.ToString();
        }
    }
}